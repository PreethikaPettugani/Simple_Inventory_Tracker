using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInventoryTracker.Data;
using SimpleInventoryTracker.DTOs;
using SimpleInventoryTracker.Models;
using SimpleInventoryTracker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleInventoryTracker.Tests
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
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Items.AddRange(
                new Item { ItemId = 1, Name = "laptop", Description = "dell i7", Quantity = 10, Category = "Electronics", MinimumStockThreshold = 5 },
                new Item { ItemId = 2, Name = "Mouse", Description = "Logitech M185", Quantity = 3, Category = "Electronics", MinimumStockThreshold = 5 },
                new Item { ItemId = 3, Name = "Office Chair", Description = "Ergonomic Black", Quantity = 5, Category = "Furniture", MinimumStockThreshold = 3 },
                new Item { ItemId = 4, Name = "pens", Description = "Blue Gel Pen", Quantity = 100, Category = "Stationery", MinimumStockThreshold = 10 },
                new Item { ItemId = 5, Name = "Notebook", Description = "A4 white NoteBook", Quantity = 20, Category = "Stationery", MinimumStockThreshold = 5 }
            );
            _context.SaveChanges();

            _service = new ItemService(_context);
        }

        [TestMethod]
        public async Task GetAllItemsAsync_ShouldReturnAllItems()
        {
            // Act
            var items = await _service.GetAllItemsAsync();

            // Assert
            Assert.AreEqual(5, items.Count); 

            var laptop = items.FirstOrDefault(i => i.ItemId == 1);
            Assert.IsNotNull(laptop);
            Assert.AreEqual("laptop", laptop.Name);
            Assert.AreEqual("dell i7", laptop.Description);
            Assert.AreEqual("Electronics", laptop.Category);
            Assert.AreEqual(10, laptop.Quantity);
            Assert.AreEqual(5, laptop.MinimumStockThreshold);
        }

        [TestMethod]
        public async Task UpdateItemAsync_ShouldUpdateItemDetails()
        {
            var updateDto = new UpdateItemDto
            {
                Name = "Updated Laptop",
                Description = "Updated Description",
                Quantity = 15,
                Category = "Electronics",
                MinimumStockThreshold = 7
            };

            var result = await _service.UpdateItemAsync(1, updateDto);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Laptop", result.Name);
            Assert.AreEqual(15, result.Quantity);
        }
        [TestMethod]
        public async Task UpdateItemQuantityAsync_ShouldUpdateOnlyQuantity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Isolated DB
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Add an item like in your DB
                context.Items.Add(new Item
                {
                    ItemId = 1,
                    Name = "laptop",
                    Description = "dell i7",
                    Quantity = 10,
                    Category = "Electronics",
                    MinimumStockThreshold = 5
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(options))
            {
                var service = new ItemService(context);

                int quantityChange = -2;
                var updatedItem = await service.UpdateItemQuantityAsync(1, quantityChange);

                // Assert
                Assert.IsNotNull(updatedItem);
                Assert.AreEqual(8, updatedItem.Quantity); 
                Assert.AreEqual("laptop", updatedItem.Name); 
            }
        }
        [TestMethod]
        public async Task GetLowStockItemsAsync_ReturnsOnlyLowStockItems()
        {
            var service = new ItemService(_context);
            // Act
            var lowStockItems = await service.GetLowStockItemsAsync();
            var resultList = lowStockItems.ToList();

            // Assert
            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual("Mouse", resultList[0].Name);
            Assert.IsTrue(resultList[0].Quantity < resultList[0].MinimumStockThreshold);
        }
        [TestMethod]
        public async Task GetItemsByCategoryAsync_ShouldReturnItemsInGivenCategory()
        {
            var service = new ItemService(_context);
            // Act
            var result = await service.GetItemsByCategoryAsync("Electronics");
            var itemList = result.ToList();

            // Assert
            Assert.AreEqual(2, itemList.Count);
            Assert.IsTrue(itemList.All(i => i.Category == "Electronics"));
            Assert.IsTrue(itemList.Any(i => i.Name == "laptop"));
            Assert.IsTrue(itemList.Any(i => i.Name == "Mouse"));
        }
       
        [TestMethod]
        public async Task DeleteItemAsync_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext(); 
            var service = new ItemService(context);

            // Act
            var result = await service.DeleteItemAsync(999); 

            // Assert
            Assert.IsFalse(result);
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }


    }
}

