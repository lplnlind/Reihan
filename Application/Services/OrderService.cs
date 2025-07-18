using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


public class OrderService : IOrderService
{
    private readonly ICartRepository _cartRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IProductImageRepository _productImageRepo;
    private readonly IOrderItemRepository _orderItemRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        ICartRepository cartRepo,
        IOrderRepository orderRepo,
        IProductRepository productRepo,
        IProductImageRepository productImageRepo,
        IOrderItemRepository orderItemRepo,
        IUserRepository userRepo,
        ILogger<OrderService> logger,
        IMapper mapper)
    {
        _cartRepo = cartRepo;
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _productImageRepo = productImageRepo;
        _orderItemRepo = orderItemRepo;
        _userRepo = userRepo;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        _logger.LogInformation("دریافت تمام سفارش‌ها");

        var orders = await _orderRepo.GetAllAsync();
        var orderIds = orders.Select(s => s.UserId).ToList();
        var users = await _userRepo.GetByIdsAsync(orderIds);
        var userDict = users.ToDictionary(u => u.Id, u => u.FullName);

        var orderItems = await _orderItemRepo.GetAllAsync();
        var itemCount = orderItems.GroupBy(i => i.OrderId)
                                  .ToDictionary(g => g.Key, g => g.Count());

        var ordersDto = _mapper.Map<List<OrderDto>>(orders);

        foreach (var item in ordersDto)
        {
            item.UserFullName = userDict.GetValueOrDefault(item.UserId, "ناشناس");
            item.OrderItemCount = itemCount.GetValueOrDefault(item.Id, 0);
        }

        return ordersDto;
    }

    public async Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId)
    {
        _logger.LogInformation("در حال دریافت جزئیات سفارش با شناسه {OrderId}", orderId);

        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order is null)
        {
            _logger.LogWarning("سفارش با شناسه {OrderId} یافت نشد", orderId);
            throw new AppException("سفارش مورد نظر یافت نشد.", StatusCodes.Status404NotFound,
                ErrorCode.OrderNotFound);
        }

        var items = await _orderItemRepo.GetByOrderIdAsync(orderId);
        var user = await _userRepo.GetByIdAsync(order.UserId);

        var orderDto = _mapper.Map<OrderDetailsDto>(order);
        orderDto.Items = _mapper.Map<List<OrderItemDto>>(items);
        orderDto.UserFullName = user?.FullName ?? "ناشناس";

        return orderDto;
    }

    public async Task UpdateOrderStatusAsync(int id, OrderStatus newStatus)
    {
        _logger.LogInformation("در حال بروزرسانی وضعیت سفارش {OrderId} به {NewStatus}", id, newStatus);

        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
        {
            _logger.LogWarning("سفارش با شناسه {OrderId} یافت نشد", id);
            throw new AppException("سفارش مورد نظر یافت نشد.", StatusCodes.Status404NotFound,
                ErrorCode.OrderNotFound);
        }

        if (!Enum.TryParse<OrderStatus>(newStatus.ToString(), true, out var parsedStatus))
        {
            _logger.LogWarning("وضعیت نامعتبر: {Status}", newStatus);
            throw new AppException("وضعیت ارسالی نامعتبر است.", StatusCodes.Status400BadRequest,
                ErrorCode.InvalidOrderStatus);
        }

        order.SetStatus(parsedStatus);
        await _orderRepo.UpdateAsync(order);

        _logger.LogInformation("وضعیت سفارش {OrderId} با موفقیت بروزرسانی شد", id);
    }

    public async Task<int> CreateOrderAsync(int userId, CreateOrderRequest request)
    {
        _logger.LogInformation("در حال ایجاد سفارش جدید برای کاربر {UserId}", userId);

        var cart = await _cartRepo.GetByUserIdAsync(userId);
        if (cart == null || !cart.Items.Any())
        {
            _logger.LogWarning("سبد خرید کاربر {UserId} خالی است", userId);
            throw new AppException("سبد خرید شما خالی است.", StatusCodes.Status400BadRequest,
                ErrorCode.CartIsEmpty);
        }

        // بررسی موجودی محصولات
        foreach (var item in cart.Items)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                _logger.LogError("محصول {ProductId} یافت نشد", item.ProductId);
                throw new AppException("یکی از محصولات موجود نیست.", StatusCodes.Status404NotFound,
                    ErrorCode.ProductNotFound);
            }

            if (product.StockQuantity < item.Quantity)
            {
                _logger.LogWarning("موجودی محصول '{ProductName}' کافی نیست", product.Name);
                throw new AppException($"موجودی محصول '{product.Name}' کافی نیست.", 400, ErrorCode.InsufficientStock);
            }
        }

        var productIds = cart.Items.Select(i => i.ProductId).ToList();
        var productImages = await _productImageRepo.GetByProductIdsAsync(productIds);
        var imageLookup = productImages.GroupBy(i => i.ProductId)
                                       .ToDictionary(g => g.Key, g => g.First().Url);

        var orderItems = _mapper.Map<List<OrderItem>>(cart.Items);
        foreach (var item in orderItems)
            item.ProductImage = imageLookup.GetValueOrDefault(item.ProductId, string.Empty);

        var address = _mapper.Map<Address>(request.ShippingAddress);

        var order = new Order(userId, address, orderItems, OrderStatus.Pending);
        await _orderRepo.AddAsync(order);

        // کاهش موجودی محصولات
        foreach (var item in cart.Items)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            product!.ReduceStock(item.Quantity);
            await _productRepo.UpdateAsync(product);
        }

        // خالی کردن سبد
        cart.Clear();
        await _cartRepo.UpdateAsync(cart);

        _logger.LogInformation("سفارش جدید با شناسه {OrderId} با موفقیت ایجاد شد", order.Id);

        return order.Id;
    }

    public async Task<List<OrderDetailsDto>> GetOrdersByUserAsync(int userId)
    {
        _logger.LogInformation("در حال دریافت سفارشات کاربر {UserId}", userId);

        var orders = await _orderRepo.GetByUserIdAsync(userId);
        if (orders == null || !orders.Any())
        {
            _logger.LogWarning("هیچ سفارشی برای کاربر {UserId} یافت نشد", userId);
            return new();
        }

        var orderIds = orders.Select(o => o.Id).ToList();
        var items = await _orderItemRepo.GetByOrderIdsAsync(orderIds);
        var groupedItems = items.GroupBy(i => i.OrderId).ToDictionary(g => g.Key, g => g.ToList());

        var ordersDto = _mapper.Map<List<OrderDetailsDto>>(orders);
        foreach (var order in ordersDto)
            order.Items = groupedItems.TryGetValue(order.Id, out var orderItems)
                ? _mapper.Map<List<OrderItemDto>>(orderItems) : new();

        return ordersDto;
    }
}
