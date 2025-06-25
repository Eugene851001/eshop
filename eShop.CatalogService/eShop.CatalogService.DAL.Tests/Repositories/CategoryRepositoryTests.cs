using eShop.CatalogService.BLL.Models;
using eShop.CatalogService.DAL.Contexts;
using eShop.CatalogService.DAL.Repositories;

using Microsoft.EntityFrameworkCore;

namespace eShop.CatalogService.DAL.Tests.Repositories;

internal class CategoryRepositoryTests
{
    private ApplicationDbContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDatabase").Options;

        _context = new ApplicationDbContext(options);

        _context.Categories.Add(new Category { Id = 1, Name = "Category1" });
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddCategory_ShouldAddCategory()
    {
        // Arrange
        var category = new Category { Id = 2, Name = "Test Category" };
        var repository = new GenericRepository<Category>(_context);

        // Act
        repository.Add(category);
        await repository.SaveAsync();

        // Assert
        var addedCategory = _context.Categories.FirstOrDefault(c => c.Id == 2);
        Assert.IsNotNull(addedCategory);
    }

    [Test]
    public async Task GetCategory_ShouldReturnCategory()
    {
        // Arrange
        var repository = new GenericRepository<Category>(_context);
        var categoryId = 1;

        // Act
        var category = await repository.GetAsync(categoryId);

        // Assert
        Assert.IsNotNull(category);
        Assert.That(category.Id, Is.EqualTo(categoryId));
    }
}
