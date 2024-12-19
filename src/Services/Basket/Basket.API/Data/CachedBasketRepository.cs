﻿using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public sealed class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
        }
        
        var basket = await repository.GetBasketAsync(userName, cancellationToken);
        
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        
        return await repository.GetBasketAsync(userName, cancellationToken);
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken)
    {
        await repository.StoreBasketAsync(basket, cancellationToken);
        
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        await repository.DeleteBasketAsync(userName, cancellationToken);
        
        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }
}