namespace Basket.API.Repositories;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        var getBastket = session.LoadAsync<ShoppingCart>(username, cancellationToken);
        if (getBastket is null)
            throw new BasketNotFoundException(username);
        session.Delete(getBastket);
        await session.SaveChangesAsync();
        return true; 
    }

    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var getBasket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
        return getBasket is null ? throw new BasketNotFoundException(userName) : getBasket;
    }

    public async Task<IReadOnlyList<ShoppingCart>> GetBasketList(CancellationToken cancellationToken)
    {
        var getBasket = await session.Query<ShoppingCart>().ToListAsync();
        return getBasket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket;
    }
}
