namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName,CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ShoppingCart>> GetBasketList(CancellationToken cancellationToken);
    Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default);
    Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default);
}
