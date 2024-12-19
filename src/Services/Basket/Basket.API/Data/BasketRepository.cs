﻿namespace Basket.API.Data;

public sealed class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);

        return basket ?? throw new BasketNotFoundException(userName);
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken)
    {
        session.Store(basket);
        
        await session.SaveChangesAsync(cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        session.Delete<ShoppingCart>(userName);
        
        await session.SaveChangesAsync(cancellationToken);

        return true;
    }
}