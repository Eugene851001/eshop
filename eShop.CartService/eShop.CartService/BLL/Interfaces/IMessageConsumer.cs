namespace eShop.CartService.BLL.Interfaces;

public interface IMessageConsumer<T> where T : class
{
    event Action<T> OnMessageReceive;

    Task StartMessageProcessing(CancellationToken cancellationToken);

    void StopMessageProcessing();
}
