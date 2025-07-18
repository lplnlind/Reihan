﻿namespace Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
