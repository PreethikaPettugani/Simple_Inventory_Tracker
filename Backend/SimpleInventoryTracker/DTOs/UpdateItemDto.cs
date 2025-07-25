using System.ComponentModel.DataAnnotations;
namespace SimpleInventoryTracker.DTOs
{
    public class UpdateItemDto
    {
        [Required]   
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string? Category { get; set; }
        public int MinimumStockThreshold { get; set; }
    }
}
