namespace eShop.CatalogService.API.Models;

public class AddCategoryResponse
{
    public int CategoryId { get; set; }

    public HateoasLink[] Links { get; set; } = Array.Empty<HateoasLink>();
}
