﻿using Azure.Core;
using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;

namespace Ordering.Api.Endpoints;

public sealed record UpdateOrderRequest(OrderDto Order);

public sealed record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateOrderResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Order")
            .WithDescription("Update Order");
    }
}