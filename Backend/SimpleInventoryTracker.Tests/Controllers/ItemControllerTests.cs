using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleInventoryTracker.Controllers;
using SimpleInventoryTracker.DTOs;
using SimpleInventoryTracker.Services.Interface;

namespace SimpleInventoryTracker.Tests.Controllers
{
    [TestClass]
    public class ItemControllerTests
    {
        private Mock<IItemService> _mockService;
        private ItemController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IItemService>();
            _controller = new ItemController(_mockService.Object);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnOkWithItems()
        {
            _mockService.Setup(s => s.GetAllItemsAsync())
                .ReturnsAsync(new List<ItemDto> { new ItemDto { ItemId = 1, Name = "Test Item" } });

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
        [TestMethod]
        public async Task GetById_ShouldReturnItem_WhenExists()
        {
            _mockService.Setup(s => s.GetItemByIdAsync(1))
                .ReturnsAsync(new ItemDto { ItemId = 1, Name = "Test Item" });

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }
        [TestMethod]
        public async Task GetById_ShouldReturnNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.GetItemByIdAsync(99)).ReturnsAsync((ItemDto?)null);

            var result = await _controller.GetById(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var createDto = new CreateItemDto { Name = "New", Category = "Electronics", Quantity = 5, Description = "Test", MinimumStockThreshold = 1 };
            var createdItem = new ItemDto { ItemId = 1, Name = "New", Category = "Electronics" };

            _mockService.Setup(s => s.CreateItemAsync(createDto)).ReturnsAsync(createdItem);

            var result = await _controller.Create(createDto);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetById", createdResult.ActionName);
        }
        [TestMethod]
        public async Task Update_ShouldReturnOk_WhenItemExists()
        {
            var updateDto = new UpdateItemDto { Name = "Updated", Description = "Updated", Quantity = 10, Category = "Electronics", MinimumStockThreshold = 2 };

            _mockService.Setup(s => s.UpdateItemAsync(1, updateDto)).ReturnsAsync(new ItemDto());

            var result = await _controller.Update(1, updateDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }
        [TestMethod]
        public async Task Update_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            var dto = new UpdateItemDto();
            _mockService.Setup(s => s.UpdateItemAsync(1, dto)).ReturnsAsync((ItemDto?)null);

            var result = await _controller.Update(1, dto);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task UpdateQuantity_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var updatedItem = new ItemDto
            {
                ItemId = 1,
                Name = "Laptop",
                Description = "Dell i7",
                Quantity = 15,
                Category = "Electronics",
                MinimumStockThreshold = 5,
                IsLowStock = false
            };
            _mockService.Setup(s => s.UpdateItemQuantityAsync(1, 5))
                        .ReturnsAsync(updatedItem);

            // Act
            var result = await _controller.UpdateQuantity(1, new UpdateItemQuantityDto { Quantity = 5 });

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(updatedItem, okResult.Value);
        }
        [TestMethod]
        public async Task UpdateQuantity_ShouldReturnBadRequest_WhenInvalid()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateItemQuantityAsync(1, -100))
                        .ReturnsAsync((ItemDto?)null);

            // Act
            var result = await _controller.UpdateQuantity(1, new UpdateItemQuantityDto { Quantity = -100 });

            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual("Invalid item ID or quantity cannot be reduced below zero.", badRequest.Value);
        }
        [TestMethod]
        public async Task GetLowStockItems_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetLowStockItemsAsync()).ReturnsAsync(new List<ItemDto>());

            var result = await _controller.GetLowStockItems();

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
        }
        [TestMethod]
        public async Task GetItemsByCategory_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetItemsByCategoryAsync("Electronics"))
                .ReturnsAsync(new List<ItemDto> { new ItemDto { ItemId = 1, Category = "Electronics" } });

            var result = await _controller.GetItemsByCategory("Electronics");

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
        }
        [TestMethod]
        public async Task Delete_ShouldReturnOk_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteItemAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
        }
        [TestMethod]
        public async Task Delete_ShouldReturnNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.DeleteItemAsync(99)).ReturnsAsync(false);

            var result = await _controller.Delete(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

    }
}
