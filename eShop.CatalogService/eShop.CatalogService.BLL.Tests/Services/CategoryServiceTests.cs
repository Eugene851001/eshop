using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Models;
using eShop.CatalogService.BLL.Services;

using Moq;

namespace eShop.CatalogService.BLL.Tests.Services;

internal class CategoryServiceTests
{
    private Mock<IRepository<Category>> _repositoryMock;
    private CategoryService _categoryService;

    [OneTimeSetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Category>>();
        _categoryService = new CategoryService(_repositoryMock.Object);
    }

    [TearDown]
    public void AfterTest()
    {
        _repositoryMock.Invocations.Clear();
    }

    [Test]
    public async Task AddCategory_EntityIsValid_ShouldAddCategory()
    {
        // Arrange
        var category = new Category { Name = "Electronics", Image = "https://example.com/image.jpg" };


        // Act
        await _categoryService.AddAsync(category);

        // Assert
        _repositoryMock.Verify(r => r.Add(It.IsAny<Category>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Test]
    public void AddCategory_ImageUrlIsInvalid_ShouldThrowException()
    {
        // Arrange
        var category = new Category { Name = "Electronics", Image = "invalid-url" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _categoryService.AddAsync(category).GetAwaiter().GetResult());
    }

    [Test]
    public async Task UpdateCategory_EntityIsValid_ShouldUpdateCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Electronics", Image = "https://example.com/image.jpg" };

        // Act
        await _categoryService.UpdateAsync(category);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task GetSingle_EntityExists_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Electronics", Image = "https://example.com/image.jpg" };
        _repositoryMock.Setup(r => r.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(category));

        // Act
        var result = await _categoryService.GetSingle(1);

        // Assert
        Assert.That(result, Is.EqualTo(category));
    }
}
