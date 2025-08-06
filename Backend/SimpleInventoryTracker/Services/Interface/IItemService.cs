using SimpleInventoryTracker.DTOs;

namespace SimpleInventoryTracker.Services.Interface
{
    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto?> GetItemByNameAsync(string name);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto);
        Task<ItemDto?> UpdateItemAsync(int id, UpdateItemDto dto);
        Task<ItemDto?> UpdateItemQuantityAsync(int id, int quantityChange);
        Task<IEnumerable<ItemDto>> GetLowStockItemsAsync();
        Task<IEnumerable<ItemDto>> GetHighStockItemsAsync();
        Task<IEnumerable<ItemDto>> GetItemsByCategoryAsync(string category);
        Task<List<string>> GetAllCategoriesAsync();
        Task<bool> DeleteItemAsync(int id);
    }
}
