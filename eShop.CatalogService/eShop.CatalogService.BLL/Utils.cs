namespace eShop.CatalogService.BLL;

public class Utils
{
    public static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, new UriCreationOptions(), out var _);
    }
}
