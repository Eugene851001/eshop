namespace eShop.CatalogService.API.Models;

public class AddProductResponse
{
    public int ProductId { get; set; }

    public HateoasLink[] Links { get; set; } = Array.Empty<HateoasLink>();
}
