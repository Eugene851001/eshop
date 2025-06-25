namespace eShop.CatalogService.BLL.Models;

public abstract class Entity
{
    public int Id { get; set; }

    public bool Deleted { get; set; }
}
