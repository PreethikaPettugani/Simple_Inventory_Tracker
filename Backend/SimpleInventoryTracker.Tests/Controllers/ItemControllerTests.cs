using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInventoryTracker.Controllers;
using SimpleInventoryTracker.DTOs;
using SimpleInventoryTracker.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleInventoryTracker.Tests.Controllers
{
    [TestClass]
    public class ItemControllerTests
    {
        private Mock<IItemService> _serviceMock;
        private ItemController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IItemService>();
            _controller = new ItemController(_serviceMock.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkWithItems()
        {
            var mockItems = new List<ItemDto> { new ItemDto { ItemId = 1, Name = "Item1" } };
            _serviceMock.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(mockItems);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(mockItems, okResult.Value);
        }

        [TestMethod]
        public async Task GetById_ReturnsOk_WhenItemFound()
        {
            var item = new ItemDto { ItemId = 1, Name = "Test" };
            _serviceMock.Setup(s => s.GetItemByIdAsync(1)).ReturnsAsync(item);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(item, okResult.Value);
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_WhenItemMissing()
        {
            _serviceMock.Setup(s => s.GetItemByIdAsync(2)).ReturnsAsync((ItemDto)null);

            var result = await _controller.GetById(2);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetByName_ReturnsOk_WhenFound()
        {
            var item = new ItemDto { ItemId = 1, Name = "Pen" };
            _serviceMock.Setup(s => s.GetItemByNameAsync("Pen")).ReturnsAsync(item);

            var result = await _controller.GetByName("Pen");

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(item, ok.Value);
        }

        [TestMethod]
        public async Task GetByName_ReturnsContent_WhenNotFound()
        {
            _serviceMock.Setup(s => s.GetItemByNameAsync("Marker")).ReturnsAsync((ItemDto)null);

            var result = await _controller.GetByName("Marker");

            Assert.IsInstanceOfType(result, typeof(ContentResult));
        }

        [TestMethod]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var dto = new CreateItemDto { Name = "NewItem", Quantity = 5 };
            var created = new ItemDto { ItemId = 2, Name = "NewItem", Quantity = 5 };

            _serviceMock.Setup(s => s.CreateItemAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Create(dto);

            var createdAt = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual("GetById", createdAt.ActionName);
            Assert.AreEqual(created, createdAt.Value);
        }

        [TestMethod]
        public async Task Update_ReturnsOk_WhenSuccess()
        {
            var dto = new UpdateItemDto { Name = "Updated", Quantity = 10 };
            var updated = new ItemDto { ItemId = 1, Name = "Updated", Quantity = 10 };

            _serviceMock.Setup(s => s.UpdateItemAsync(1, dto)).ReturnsAsync(updated);

            var result = await _controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual("Item Updated Sucessfully...!", ok.Value);
        }

        [TestMethod]
        public async Task Update_ReturnsNotFound_WhenNull()
        {
            _serviceMock.Setup(s => s.UpdateItemAsync(99, It.IsAny<UpdateItemDto>())).ReturnsAsync((ItemDto)null);

            var result = await _controller.Update(99, new UpdateItemDto());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateQuantity_ReturnsOk_WhenValid()
        {
            var dto = new UpdateItemQuantityDto { Quantity = 3 };
            var updated = new ItemDto { ItemId = 1, Quantity = 3 };

            _serviceMock.Setup(s => s.UpdateItemQuantityAsync(1, 3)).ReturnsAsync(updated);

            var result = await _controller.UpdateQuantity(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(updated, ok.Value);
        }

        [TestMethod]
        public async Task UpdateQuantity_ReturnsBadRequest_WhenInvalid()
        {
            _serviceMock.Setup(s => s.UpdateItemQuantityAsync(2, 5)).ReturnsAsync((ItemDto)null);

            var result = await _controller.UpdateQuantity(2, new UpdateItemQuantityDto { Quantity = 5 });

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetLowStockItems_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetLowStockItemsAsync()).ReturnsAsync(new List<ItemDto>());

            var result = await _controller.GetLowStockItems();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetHighStockItems_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetHighStockItemsAsync()).ReturnsAsync(new List<ItemDto>());

            var result = await _controller.GetHighStockItems();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetItemsByCategory_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetItemsByCategoryAsync("Stationery")).ReturnsAsync(new List<ItemDto>());

            var result = await _controller.GetItemsByCategory("Stationery");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAllCategories_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetAllCategoriesAsync()).ReturnsAsync(new List<string>());

            var result = await _controller.GetAllCategories();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            _serviceMock.Setup(s => s.DeleteItemAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual("Item Deleted Successfully...!", ok.Value);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.DeleteItemAsync(2)).ReturnsAsync(false);

            var result = await _controller.Delete(2);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}


