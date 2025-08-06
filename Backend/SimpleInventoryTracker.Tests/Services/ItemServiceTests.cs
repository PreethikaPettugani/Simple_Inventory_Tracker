using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SimpleInventoryTracker.Data;
using SimpleInventoryTracker.DTOs;
using SimpleInventoryTracker.Models;
using SimpleInventoryTracker.Services;

namespace SimpleInventoryTracker.Tests.Services
{
    [TestClass]
    public class ItemServiceTests
    {
        private AppDbContext _context;
        private ItemService _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated(); 

            _service = new ItemService(_context);
        }


        [TestMethod]
        public async Task CreateItemAsync_Should_Add_Item()
        {
            var dto = new CreateItemDto
            {
                ItemId = 1,
                Name = "Test Item",
                Description = "Test Description",
                Quantity = 10,
                Category = "Category A",
                MinimumStockThreshold = 3
            };

            var result = await _service.CreateItemAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Item", result.Name);
        }

        [TestMethod]
        public async Task GetAllItemsAsync_Should_Return_All_Items()
        {
            _context.Items.AddRange(new List<Item>
            {
                new Item { ItemId = 1, Name = "Item1", Quantity = 5, Category = "A", MinimumStockThreshold = 2 },
                new Item { ItemId = 2, Name = "Item2", Quantity = 8, Category = "B", MinimumStockThreshold = 4 }
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetAllItemsAsync();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetItemByIdAsync_Should_Return_Correct_Item()
        {
            _context.Items.Add(new Item
            {
                ItemId = 100,
                Name = "Item100",
                Quantity = 7,
                Category = "CategoryX",
                MinimumStockThreshold = 3
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetItemByIdAsync(100);

            Assert.IsNotNull(result);
            Assert.AreEqual("Item100", result.Name);
        }

        [TestMethod]
        public async Task GetItemByNameAsync_Should_Return_Correct_Item()
        {
            _context.Items.Add(new Item
            {
                ItemId = 101,
                Name = "UniqueName",
                Quantity = 5,
                Category = "Y",
                MinimumStockThreshold = 2
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetItemByNameAsync("UniqueName");

            Assert.IsNotNull(result);
            Assert.AreEqual(101, result.ItemId);
        }

        [TestMethod]
        public async Task UpdateItemAsync_Should_Update_All_Fields()
        {
            _context.Items.Add(new Item
            {
                ItemId = 102,
                Name = "OldName",
                Description = "OldDesc",
                Quantity = 10,
                Category = "C",
                MinimumStockThreshold = 3
            });
            await _context.SaveChangesAsync();

            var dto = new UpdateItemDto
            {
                Name = "UpdatedName",
                Description = "UpdatedDesc",
                Quantity = 20,
                Category = "UpdatedCategory",
                MinimumStockThreshold = 5
            };

            var result = await _service.UpdateItemAsync(102, dto);

            Assert.IsNotNull(result);
            Assert.AreEqual("UpdatedName", result.Name);
            Assert.AreEqual(20, result.Quantity);
        }

        [TestMethod]
        public async Task UpdateItemQuantityAsync_Should_Add_Quantity()
        {
            _context.Items.Add(new Item
            {
                ItemId = 103,
                Name = "QuantityItem",
                Quantity = 10,
                Category = "Z",
                MinimumStockThreshold = 5
            });
            await _context.SaveChangesAsync();

            var result = await _service.UpdateItemQuantityAsync(103, 5);

            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.Quantity);
        }

        [TestMethod]
        public async Task UpdateItemQuantityAsync_Should_Return_Null_For_Negative_Quantity()
        {
            _context.Items.Add(new Item
            {
                ItemId = 104,
                Name = "NegativeItem",
                Quantity = 5,
                MinimumStockThreshold = 2
            });
            await _context.SaveChangesAsync();

            var result = await _service.UpdateItemQuantityAsync(104, -10);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetLowStockItemsAsync_Should_Return_Items_Below_Threshold()
        {
            _context.Items.AddRange(new[]
            {
                new Item { ItemId = 105, Name = "Low", Quantity = 1, MinimumStockThreshold = 3 },
                new Item { ItemId = 106, Name = "Enough", Quantity = 5, MinimumStockThreshold = 3 }
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetLowStockItemsAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Low", result.First().Name);
        }

        [TestMethod]
        public async Task GetHighStockItemsAsync_Should_Return_Items_Above_Threshold()
        {
            _context.Items.AddRange(new[]
            {
                new Item { ItemId = 107, Name = "High1", Quantity = 10, MinimumStockThreshold = 3 },
                new Item { ItemId = 108, Name = "Low", Quantity = 2, MinimumStockThreshold = 4 }
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetHighStockItemsAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("High1", result.First().Name);
        }

        [TestMethod]
        public async Task GetItemsByCategoryAsync_Should_Filter_By_Category()
        {
            _context.Items.AddRange(new[]
            {
                new Item { ItemId = 109, Name = "ItemA", Category = "Cat1" },
                new Item { ItemId = 110, Name = "ItemB", Category = "Cat2" },
                new Item { ItemId = 111, Name = "ItemC", Category = "Cat1" }
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetItemsByCategoryAsync("Cat1");

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(i => i.Category == "Cat1"));
        }

        [TestMethod]
        public async Task GetAllCategoriesAsync_Should_Return_Distinct_Categories()
        {
            _context.Items.AddRange(new[]
            {
                new Item { ItemId = 112, Name = "One", Category = "Alpha" },
                new Item { ItemId = 113, Name = "Two", Category = "Beta" },
                new Item { ItemId = 114, Name = "Three", Category = "Alpha" }
            });
            await _context.SaveChangesAsync();

            var result = await _service.GetAllCategoriesAsync();

            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, "Alpha");
            CollectionAssert.Contains(result, "Beta");
        }

        [TestMethod]
        public async Task DeleteItemAsync_Should_Remove_Existing_Item()
        {
            _context.Items.Add(new Item { ItemId = 115, Name = "ToDelete" });
            await _context.SaveChangesAsync();

            var result = await _service.DeleteItemAsync(115);

            Assert.IsTrue(result);
            Assert.AreEqual(0, _context.Items.Count());
        }

        [TestMethod]
        public async Task DeleteItemAsync_Should_Return_False_For_NonExisting()
        {
            var result = await _service.DeleteItemAsync(999);

            Assert.IsFalse(result);
        }
    }
}

