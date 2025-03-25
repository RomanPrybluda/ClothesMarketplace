﻿namespace DAL
{
    public class DeliveryMethod
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Ad> Ads { get; set; } = new();
    }
}
