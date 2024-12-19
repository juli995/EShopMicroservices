namespace Basket.API.Features.Basket.GetBasket;

public sealed record GetBasketRequest(
    string UserName
);

public sealed record GetBasketResponse(
    ShoppingCart Cart
);

public sealed class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                
                var response = result.Adapt<GetBasketResponse>();
                
                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>()
            .Produces<GetBasketResponse>(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");
    }
}