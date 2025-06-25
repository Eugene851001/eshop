using eShop.CatalogService.API.Controllers.v1;
using eShop.CatalogService.BLL.Features.Category.Commands;
using eShop.CatalogService.BLL.Features.Category.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace eShop.CatalogService.API.Tests.Controllers;

public class CategoriesControllerTests
{
    [Test]
    public async Task CategoriesController_GetCategories()
    {
        //arrange
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()));

        var controller = new CategoriesController(mediatorMock.Object);

        //act
        var result = await controller.GetCategories();

        //assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        mediatorMock.Verify(m => m.Send(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CategoriesController_DeleteCategory()
    {
        //arrange
        var mediatorMock = new Mock<IMediator>();

        var command = new RemoveCategoryCommand { Id = 1 };
        mediatorMock.Setup(m => m.Send(It.IsAny<RemoveCategoryCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var controller = new CategoriesController(mediatorMock.Object);

        //act
        var result = await controller.DeleteCategory(1);

        //assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<OkResult>());

        mediatorMock.Verify(m => m.Send(It.IsAny<RemoveCategoryCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
