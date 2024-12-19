﻿namespace Basket.API.Features.Basket.DeleteBasket;

public sealed record DeleteBasketResponse(
    bool IsSuccess
);

public sealed class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                
                var response = result.Adapt<DeleteBasketResponse>();
                
                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>()
            .Produces<DeleteBasketResponse>(StatusCodes.Status400BadRequest)
            .Produces<DeleteBasketResponse>(StatusCodes.Status404NotFound)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
    }
}