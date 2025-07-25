using SimpleInventoryTracker.DTOs;

namespace SimpleInventoryTracker.Services.Interface
{
    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto);
        Task<ItemDto?> UpdateItemAsync(int id, UpdateItemDto dto);
        Task<(bool Success, string Message)> UpdateItemQuantityAsync(int id, int quantityChange);
        Task<IEnumerable<ItemDto>> GetLowStockItemsAsync();
        Task<IEnumerable<ItemDto>> GetItemsByCategoryAsync(string category);

        Task<bool> DeleteItemAsync(int id);

    }
}
