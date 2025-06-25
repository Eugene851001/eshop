namespace eShop.CatalogService.BLL.Interfaces;

public interface IMessageProducer<T> where T : class
{
    Task Produce(T message);
}
