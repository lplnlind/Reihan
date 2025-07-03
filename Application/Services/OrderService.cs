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
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IUserRepository _userRepo;

        public OrderService(ICartRepository cartRepo,
            IOrderRepository orderRepo,
            IProductRepository productRepo,
            IOrderItemRepository orderItemRepo,
            IUserRepository userRepo)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
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

            // ساخت OrderItem
            var orderItems = cart.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList();

            var address = new Address(request.ShippingAddress.Street, request.ShippingAddress.City, request.ShippingAddress.ZipCode);
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

        public async Task<List<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalAmount
            }).ToList();
        }
    }
}
