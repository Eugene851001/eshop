namespace eShop.CatalogService.BLL.Tests;

public class UtilsTest
{
    [TestCase("https://learn.epam.com/")]
    [TestCase("http://learn.epam.com/")]
    [TestCase("https://learn.epam.com/1234567890")]
    public void IsValidUrl_UrlIValid_ShouldReturnTrue(string url)
    {
        var result = Utils.IsValidUrl(url);

        Assert.True(result);
    }

    [TestCase("xcdcs")]
    [TestCase("https:/learn.epam.com/")]
    [TestCase("")]
    public void IsValUrl_UrlIsNotValid_ShouldReturnFalse(string url)
    {
        var result = Utils.IsValidUrl(url);

        Assert.False(result);
    }
}
