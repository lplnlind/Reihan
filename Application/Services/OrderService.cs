using Application.Interfaces.Repositories;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Persistence.Repositories;
using Application.DTOs;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _productImageRepo;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IUserRepository _userRepo;

        public OrderService(ICartRepository cartRepo,
            IOrderRepository orderRepo,
            IProductRepository productRepo,
            IProductImageRepository productImageRepo,
            IOrderItemRepository orderItemRepo,
            IUserRepository userRepo)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _orderItemRepo = orderItemRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();

            var users = await _userRepo.GetAllAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u.FullName);

            var orderItems = await _orderItemRepo.GetAllAsync();
            var orderItemCount = orderItems
                .GroupBy(p => p.OrderId)
                .ToDictionary(g => g.Key, g => g.Count());

            return orders.Select(c => new OrderDto
            {
                Id = c.Id,
                OrderDate = c.OrderDate,
                TotalAmount = c.TotalAmount,
                Status = c.Status.ToString(),
                City = c.ShippingAddress.City,
                ZipCode = c.ShippingAddress.ZipCode,
                UserId = c.UserId,
                UserFullName = userDict.GetValueOrDefault(c.UserId, "ناشناس"),
                OrderItemCount = orderItemCount.GetValueOrDefault(c.Id, 0),
            });
        }

        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order is null) return null;

            var items = await _orderItemRepo.GetByOrderIdAsync(orderId);

            return new OrderDetailsDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                Items = items.Select(i =>
                {
                    return new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        ProductName = i.ProductName,
                        ProductImage = i.ProductImage,
                    };
                }).ToList()
            };
        }

        public async Task UpdateOrderStatusAsync(int Id, string newStatus)
        {
            var order = await _orderRepo.GetByIdAsync(Id);
            if (order == null)
                throw new Exception("سفارش یافت نشد.");

            if (!Enum.TryParse<OrderStatus>(newStatus, out var status))
                throw new Exception("وضعیت نامعتبر است.");

            order.SetStatus(status);
            await _orderRepo.UpdateAsync(order);
        }

        public async Task<int> CreateOrderAsync(int userId, CreateOrderRequest request)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
                throw new Exception("سبد خرید خالی است.");

            // بررسی موجودی
            foreach (var item in cart.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                    throw new Exception($"موجودی محصول '{product?.Name}' کافی نیست.");
            }

            // گرفتن عکس‌ها
            var productImages = await _productImageRepo
                .GetByProductIdsAsync(cart.Items.Select(s => s.ProductId).ToList());
            var imageLookup = productImages
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.First().Url);

            // ساخت OrderItem
            var orderItems = cart.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ProductImage = imageLookup.GetValueOrDefault(x.ProductId, string.Empty)
            }).ToList();

            var address = new Address(request.ShippingAddress.State, request.ShippingAddress.Street, request.ShippingAddress.City, request.ShippingAddress.ZipCode);
            var order = new Order(userId, address, orderItems, OrderStatus.Pending);
            await _orderRepo.AddAsync(order);

            // کاهش موجودی محصول
            foreach (var item in cart.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                product.ReduceStock(item.Quantity);
            }

            // خالی کردن سبد
            cart.Clear();
            //await _unitOfWork.SaveChangesAsync();

            return order.Id;
        }

        public async Task<List<OrderDetailsDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            if (orders == null || !orders.Any())
                return new();

            var orderIds = orders.Select(o => o.Id).ToList();

            var orderItems = await _orderItemRepo.GetByOrderIdsAsync(orderIds);
            var itemsGrouped = orderItems.GroupBy(i => i.OrderId).ToDictionary(g => g.Key, g => g.ToList());

            return orders.Select(order => new OrderDetailsDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                Items = itemsGrouped.TryGetValue(order.Id, out var items)
                    ? items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        ProductImage = i.ProductImage,
                    }).ToList()
                    : new()
            }).ToList();
        }
    }
}
