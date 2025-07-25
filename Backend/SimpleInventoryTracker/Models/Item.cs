﻿using System.ComponentModel.DataAnnotations;
namespace SimpleInventoryTracker.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? Category { get; set; }
        public int MinimumStockThreshold { get; set; }
        public bool IsLowStock => Quantity < MinimumStockThreshold;

    }
}
