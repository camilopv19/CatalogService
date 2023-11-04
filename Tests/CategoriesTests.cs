using BusinessLogicLayer;
using CatalogService.Controllers;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
#pragma warning disable 1591
    public class CategoriesTests
    {

        [Fact]
        public void Get_ReturnsCategorys()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(service => service.List()).Returns(new List<Category>());
            var controller = new CategoryController(categoryServiceMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsAssignableFrom<IEnumerable<Category>>(result);
            var categories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult);
            Assert.Empty(categories);
        }

        [Fact]
        public void Get_ReturnsCategory_WhenValidIdProvided()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(service => service.Get(It.IsAny<int>())).Returns(new Category());
            var controller = new CategoryController(categoryServiceMock.Object);

            // Act
            var result = controller.Get(1); // Provide a valid ID

            // Assert
            Assert.IsType<ActionResult<Category>>(result);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenInvalidIdProvided()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(service => service.Get(It.IsAny<int>())).Returns(null as Category);
            var controller = new CategoryController(categoryServiceMock.Object);

            // Act
            var result = controller.Get(999); // Provide an invalid ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Get_ReturnsOkResult_WhenCategoryFound()
        {
            // Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var controller = new CategoryController(categoryServiceMock.Object);
            var categoryId = 1; // Sample category ID for testing
            var categoryToReturn = new Category
            {
                Id = categoryId,
                Name = "Test Category",
                Image = "",
                ParentCategoryId = 0,
            };

            categoryServiceMock.Setup(service => service.Get(categoryId)).Returns(categoryToReturn);

            // Act
            var result = controller.Get(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(categoryToReturn, returnedCategory); // Compare the returned category with the expected category
        }

        [Fact]
        public void Insert_ReturnsBadRequest()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Upsert method to return the default id (e.g., 0)
            mockCategoryService.Setup(service => service.Upsert(It.IsAny<Category>()))
                .Returns(0);  // Assuming 0 indicates a failure

            var category = new Category();  // You can create an Category object with appropriate data

            // Act
            var result = controller.Insert(category).Result as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); //Bad Request status code
        }
        [Fact]
        public void Insert_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Upsert method to return a non-default id
            mockCategoryService.Setup(service => service.Upsert(It.IsAny<Category>()))
                .Returns(42);  // Change this value to a non-default id

            var category = new Category();  // You can create an Category object with appropriate data

            // Act
            var result = controller.Insert(category).Result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode); // 201 corresponds to Created status code
            Assert.Equal("Insert", result.ActionName);
        }

        [Fact]
        public void Update_ReturnsNoContent()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Upsert method to return a positive result
            mockCategoryService.Setup(service => service.Upsert(It.IsAny<Category>()))
                .Returns(1);  // Positive result

            var category = new Category();  // You can create an Category object with appropriate data

            // Act
            var result = controller.Update(category).Result as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // 204 corresponds to No content status code (update Ok)
        }

        [Fact]
        public void Update_ReturnsNotFound()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Upsert method to return 0, indicating a failure
            mockCategoryService.Setup(service => service.Upsert(It.IsAny<Category>()))
                .Returns(0);  // Failure result

            var category = new Category();  // You can create an Category object with appropriate data

            // Act
            var result = controller.Update(category).Result as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); //Not Found status code
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Delete method to return a value greater than 0
            mockCategoryService.Setup(service => service.Delete(It.IsAny<int>()))
                .Returns(1);  // Change this value to a positive value indicating success

            var categoryId = 1;  // Replace with the ID of the category to be deleted

            // Act
            var result = controller.Delete(categoryId).Result as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // 204 corresponds to No content status code (update Ok)
        }

        [Fact]
        public void Delete_ReturnsNotFound()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Delete method to return 0 to simulate failure
            mockCategoryService.Setup(service => service.Delete(It.IsAny<int>()))
                .Returns(0);  // Assuming 0 indicates failure

            var categoryId = 1;  // Replace with the ID of the category to be deleted

            // Act
            var result = controller.Delete(categoryId).Result as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); //Not Found status code
        }

        [Fact]
        public void GetById_ReturnsOk()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Get method to return a non-default result
            var mockCategory = new Category { Id = 1, Name = "Sample Category" };  // Replace with your category details
            mockCategoryService.Setup(service => service.Get(It.IsAny<int>()))
                .Returns(mockCategory);

            var categoryId = 1;  // Replace with the ID of the category to retrieve

            // Act
            var result = controller.Get(categoryId).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var categoryResult = result.Value as Category;
            Assert.NotNull(categoryResult);
            Assert.Equal(200, result.StatusCode); // 200 corresponds to Ok
        }

        [Fact]
        public void GetById_ReturnsNotFound()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new CategoryController(mockCategoryService.Object);

            // Mock the Get method to return the default (null or default value)
            mockCategoryService.Setup(service => service.Get(It.IsAny<int>()))
                .Returns(default(Category));  // Assumes the default value for Category is null

            var categoryId = 1;  // Replace with the ID of a non-existent category

            // Act
            var result = controller.Get(categoryId).Result as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); //Not Found status code
        }

        [Fact]
        public void DeleteCategoryWithItems_DeletesItemsWithSameCategoryId()
        {
            // Arrange
            var categoryId = 1;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "in-memory-db")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Categories.Add(new Category { Id = categoryId });
                context.Items.Add(new Item { Id = 1, CategoryId = categoryId });
                context.Items.Add(new Item { Id = 2, CategoryId = categoryId });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var categoryRepository = new CategoryRepository(context);
                var result = categoryRepository.Delete(categoryId);

                // Assert
                Assert.Equal(3, result); // Total changes, including both category and items

                Assert.Null(context.Categories.Find(categoryId));
                Assert.Null(context.Items.Find(1));
                Assert.Null(context.Items.Find(2));
            }
        }
    }
}

