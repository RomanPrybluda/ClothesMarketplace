﻿namespace DAL
{
    public class Ad
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public string ProductLocation { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Product Product { get; set; }

        public Guid DeliveryMethodId { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } = null!;
    }
}

