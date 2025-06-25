namespace eShop.CatalogService.BLL.Wrappers;

public class PagedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public required T Data { get; set; }
}
