using BusinessLogicLayer;
using CatalogService.Controllers;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591
namespace Tests
{
    public class TestItems
    {

        [Fact]
        public void Get_ReturnsItems()
        {
            // Arrange
            var itemServiceMock = new Mock<IItemService>();
            itemServiceMock.Setup(service => service.List(null, null)).Returns(new List<ItemResponse>());
            var controller = new ItemsController(itemServiceMock.Object);

            // Act
            var result = controller.Get(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<ItemResponse>>(okResult.Value);
            Assert.Empty(items);
        }

        [Fact]
        public void Get_ReturnsItem_WhenValidIdProvided()
        {
            // Arrange
            var itemServiceMock = new Mock<IItemService>();
            itemServiceMock.Setup(service => service.Get(It.IsAny<int>())).Returns(new Item());
            var controller = new ItemsController(itemServiceMock.Object);

            // Act
            var result = controller.Get(1); // Provide a valid ID

            // Assert
            Assert.IsType<ActionResult<Item>>(result);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenInvalidIdProvided()
        {
            // Arrange
            var itemServiceMock = new Mock<IItemService>();
            itemServiceMock.Setup(service => service.Get(It.IsAny<int>())).Returns(null as Item);
            var controller = new ItemsController(itemServiceMock.Object);

            // Act
            var result = controller.Get(999); // Provide an invalid ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Get_ReturnsOkResult_WhenItemFound()
        {
            // Arrange
            var itemServiceMock = new Mock<IItemService>();
            var controller = new ItemsController(itemServiceMock.Object);
            var itemId = 1; // Sample item ID for testing
            var itemToReturn = new Item
            {
                Id = itemId,
                Name = "Test Item",
                Amount = 0,
                CategoryId = 0,
                Description = "",
                Image = "",
                Price = 0
            };

            itemServiceMock.Setup(service => service.Get(itemId)).Returns(itemToReturn);

            // Act
            var result = controller.Get(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItem = Assert.IsType<Item>(okResult.Value);
            Assert.Equal(itemToReturn, returnedItem); // Compare the returned item with the expected item
        }

        [Fact]
        public void Insert_ReturnsBadRequest()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Upsert method to return the default id (e.g., 0)
            mockItemService.Setup(service => service.Upsert(It.IsAny<Item>()))
                .Returns(0);  // Assuming 0 indicates a failure

            var item = new Item();  // You can create an Item object with appropriate data

            // Act
            var result = controller.Insert(item).Result as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); //Bad Request status code
        }
        [Fact]
        public void Insert_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Upsert method to return a non-default id
            mockItemService.Setup(service => service.Upsert(It.IsAny<Item>()))
                .Returns(42);  // Change this value to a non-default id

            var item = new Item();  // You can create an Item object with appropriate data

            // Act
            var result = controller.Insert(item).Result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode); // 201 corresponds to Created status code
            Assert.Equal("Insert", result.ActionName);
        }

        [Fact]
        public void Update_ReturnsNoContent()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Upsert method to return a positive result
            mockItemService.Setup(service => service.Upsert(It.IsAny<Item>()))
                .Returns(1);  // Positive result

            var item = new Item();  // You can create an Item object with appropriate data

            // Act
            var result = controller.Update(item).Result as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // 204 corresponds to No content status code (update Ok)
        }

        [Fact]
        public void Update_ReturnsNotFound()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Upsert method to return 0, indicating a failure
            mockItemService.Setup(service => service.Upsert(It.IsAny<Item>()))
                .Returns(0);  // Failure result

            var item = new Item();  // You can create an Item object with appropriate data

            // Act
            var result = controller.Update(item).Result as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); //Not Found status code
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Delete method to return a value greater than 0
            mockItemService.Setup(service => service.Delete(It.IsAny<int>()))
                .Returns(1);  // Change this value to a positive value indicating success

            var itemId = 1;  // Replace with the ID of the item to be deleted

            // Act
            var result = controller.Delete(itemId).Result as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // 204 corresponds to No content status code (update Ok)
        }

        [Fact]
        public void Delete_ReturnsNotFound()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Delete method to return 0 to simulate failure
            mockItemService.Setup(service => service.Delete(It.IsAny<int>()))
                .Returns(0);  // Assuming 0 indicates failure

            var itemId = 1;  // Replace with the ID of the item to be deleted

            // Act
            var result = controller.Delete(itemId).Result as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); //Not Found status code
        }

        [Fact]
        public void GetById_ReturnsOk()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Get method to return a non-default result
            var mockItem = new Item { Id = 1, Name = "Sample Item" };  // Replace with your item details
            mockItemService.Setup(service => service.Get(It.IsAny<int>()))
                .Returns(mockItem);

            var itemId = 1;  // Replace with the ID of the item to retrieve

            // Act
            var result = controller.Get(itemId).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var itemResult = result.Value as Item;
            Assert.NotNull(itemResult);
            Assert.Equal(200, result.StatusCode); // 200 corresponds to Ok
        }

        [Fact]
        public void GetById_ReturnsNotFound()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var controller = new ItemsController(mockItemService.Object);

            // Mock the Get method to return the default (null or default value)
            mockItemService.Setup(service => service.Get(It.IsAny<int>()))
                .Returns(default(Item));  // Assumes the default value for Item is null

            var itemId = 1;  // Replace with the ID of a non-existent item

            // Act
            var result = controller.Get(itemId).Result as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); //Not Found status code
        }
    }
}
