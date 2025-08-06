using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleInventoryTracker.DTOs;
using SimpleInventoryTracker.Services;
using SimpleInventoryTracker.Services.Interface;

namespace SimpleInventoryTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemController(IItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetItemByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpGet("byname/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var item = await _service.GetItemByNameAsync(name);
            if (item == null)
            {
                return Content($"Item '{name}' not found"); 
            }
            return Ok(item); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemDto dto)
        {
            var newItem = await _service.CreateItemAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newItem.ItemId }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateItemDto dto)
        {
            var updated = await _service.UpdateItemAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok("Item Updated Sucessfully...!");
        }

        [HttpPatch("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] UpdateItemQuantityDto dto)
        {
            var updatedItem = await _service.UpdateItemQuantityAsync(id, dto.Quantity);

            if (updatedItem == null)
            {
                return BadRequest("Invalid item ID or quantity cannot be reduced below zero.");
            }

            return Ok(updatedItem);
        }


        [HttpGet("lowstock")]
        public async Task<IActionResult> GetLowStockItems()
        {
            var items = await _service.GetLowStockItemsAsync();
            return Ok(items);
        }
        [HttpGet("highstock")]
        public async Task<IActionResult> GetHighStockItems()
        {
            var items = await _service.GetHighStockItemsAsync();
            return Ok(items);
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetItemsByCategory([FromQuery] string name)
        {
            var items = await _service.GetItemsByCategoryAsync(name);
            return Ok(items);
        }

        [HttpGet("allCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteItemAsync(id);

            if (!deleted)
                return NotFound($"Item with ID {id} not found.");

            return Ok("Item Deleted Successfully...!");
        }

    }
}


