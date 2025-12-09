using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories;

public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache distributedCache) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cacheBasket = await distributedCache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cacheBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cacheBasket)!;

        var basket = await basketRepository.GetBasket(userName, cancellationToken);
        await distributedCache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var createBasket = await basketRepository.StoreBasket(basket, cancellationToken);
        await distributedCache.SetStringAsync(createBasket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return createBasket;
    }
    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        var basket = await basketRepository.DeleteBasket(username, cancellationToken);
        await distributedCache.RemoveAsync(username, cancellationToken);

        string cacheKey = "users_all";
        string? cachedData = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
        IReadOnlyList<ShoppingCart> queryDataCache = [];
        if (!string.IsNullOrEmpty(cachedData))
        {
            queryDataCache = JsonSerializer.Deserialize<IReadOnlyList<ShoppingCart>>(cachedData)!;
            queryDataCache = queryDataCache.Where(u => !string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase)).ToList();
            await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(queryDataCache), cancellationToken);
        }
        return basket;
    }

    public async Task<IReadOnlyList<ShoppingCart>> GetBasketList(CancellationToken cancellationToken)
    {
        string cacheKey = "users_all";
        string? cachedData = await distributedCache.GetStringAsync(cacheKey,cancellationToken);
        if (!string.IsNullOrEmpty(cachedData))
            return JsonSerializer.Deserialize<IReadOnlyList<ShoppingCart>>(cachedData)!;

        var basket = await basketRepository.GetBasketList(cancellationToken);
        await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }
}

