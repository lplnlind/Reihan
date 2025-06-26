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

        public OrderService(ICartRepository cartRepo, IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
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
