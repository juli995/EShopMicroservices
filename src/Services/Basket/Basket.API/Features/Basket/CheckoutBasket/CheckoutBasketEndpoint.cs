using Basket.API.Dtos;

namespace Basket.API.Features.Basket.CheckoutBasket;

public sealed record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);

public sealed record CheckoutBasketResponse(bool IsSuccess);

public sealed class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<CheckoutBasketCommand>();
                
                var result = await sender.Send(command);
                
                var response = result.Adapt<CheckoutBasketResponse>();
                
                return Results.Ok(response);
            })
            .WithName("CheckoutBasket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .Produces<CheckoutBasketResponse>(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout Basket")
            .WithDescription("Checkout Basket");
    }
}