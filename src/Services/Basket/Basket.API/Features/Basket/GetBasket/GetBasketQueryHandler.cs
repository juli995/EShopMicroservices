using Basket.API.Data;

namespace Basket.API.Features.Basket.GetBasket;

public sealed record GetBasketQuery(string UserName)
    : IQuery<GetBasketQueryResult>;

public sealed record GetBasketQueryResult(ShoppingCart Cart);

public sealed class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketQueryResult>
{
    public async Task<GetBasketQueryResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(request.UserName, cancellationToken);

        return new GetBasketQueryResult(basket);
    }
}