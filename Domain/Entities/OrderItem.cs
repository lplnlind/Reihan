﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FinalPrice { get; set; }

        // ارتباط با Order و Product
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
