using SimpleInventoryTracker.Data;
using SimpleInventoryTracker.DTOs;
using SimpleInventoryTracker.Models;
using SimpleInventoryTracker.Services.Interface;
using System;
using Microsoft.EntityFrameworkCore;



namespace SimpleInventoryTracker.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            return await _context.Items
                .Select(item => new ItemDto
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Category = item.Category,
                    MinimumStockThreshold = item.MinimumStockThreshold,
                    IsLowStock = item.IsLowStock
                }).ToListAsync();
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return null;

            return new ItemDto
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                Quantity = item.Quantity,
                Category = item.Category,
                MinimumStockThreshold = item.MinimumStockThreshold,
                IsLowStock = item.IsLowStock
            };
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemDto dto)
        {
            var item = new Item
            {
                ItemId = dto.ItemId,
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Quantity,
                Category = dto.Category,
                MinimumStockThreshold = dto.MinimumStockThreshold
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return await GetItemByIdAsync(item.ItemId);

        }

        public async Task<ItemDto?> UpdateItemAsync(int id, UpdateItemDto dto)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return null;

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Quantity = dto.Quantity;
            item.Category = dto.Category;
            item.MinimumStockThreshold = dto.MinimumStockThreshold;

            await _context.SaveChangesAsync();

            return await GetItemByIdAsync(item.ItemId);
        }

        public async Task<ItemDto?> UpdateItemQuantityAsync(int id, int quantityChange)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return null;

            int newQuantity = item.Quantity + quantityChange;
            if (newQuantity < 0) return null;

            item.Quantity = newQuantity;

            await _context.SaveChangesAsync();

            return await GetItemByIdAsync(item.ItemId);
        }

        public async Task<IEnumerable<ItemDto>> GetLowStockItemsAsync()
        {
            var lowStockItems = await _context.Items
                .Where(item => item.Quantity < item.MinimumStockThreshold)
                .ToListAsync();

            return lowStockItems.Select(item => new ItemDto
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Quantity = item.Quantity,
                Description = item.Description,
                Category = item.Category,
                MinimumStockThreshold = item.MinimumStockThreshold,
                IsLowStock = item.IsLowStock
            });
        }
        public async Task<IEnumerable<ItemDto>> GetItemsByCategoryAsync(string category)
        {
            var query = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(item => item.Category == category);
            }

            var result = await query.ToListAsync();

            return result.Select(item => new ItemDto
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Quantity = item.Quantity,
                Description = item.Description,
                Category = item.Category,
                MinimumStockThreshold = item.MinimumStockThreshold,
                IsLowStock = item.IsLowStock
            });
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return false;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}