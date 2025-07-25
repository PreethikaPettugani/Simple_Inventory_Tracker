using System.ComponentModel.DataAnnotations;
namespace SimpleInventoryTracker.DTOs
{
    public class UpdateItemQuantityDto
    {
        [Required]
        public int Quantity { get; set; }
    }
}
