namespace Basket.API.Features.Basket.StoreBasket;

public sealed record StoreBasketRequest(
    ShoppingCart Cart
);

public sealed record StoreBasketResponse(
    ShoppingCart Cart
);

public sealed class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                
                var result = await sender.Send(command);
                
                var response = result.Adapt<StoreBasketResponse>();
                
                return Results.Ok(response);
            })
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>()
            .Produces<StoreBasketResponse>(StatusCodes.Status400BadRequest)
            .WithSummary("Store Basket")
            .WithDescription("Store Basket");
    }
}