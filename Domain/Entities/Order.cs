﻿using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System.Data;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; private set; }
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
        public Address ShippingAddress { get; private set; }
        public decimal TotalAmount { get; private set; }
        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public User User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new();

        private Order() { }

        public Order(int userId, Address shippingAddress, List<OrderItem> items, OrderStatus orderStatus)
        {
            UserId = userId;
            ShippingAddress = shippingAddress;
            OrderItems = items;
            TotalAmount = OrderItems.Sum(x => x.FinalPrice * x.Quantity);
            Status = orderStatus;
        }

        public void SetStatus(OrderStatus status)
        {
            Status = status;
            SetUpdated();
        }
    }
}
