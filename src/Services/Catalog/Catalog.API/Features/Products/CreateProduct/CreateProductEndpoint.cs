﻿using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Products.CreateProduct;

public sealed record CreateProductRequest(
    Guid Id,
    string? Name,
    List<string> Category,
    string? Description,
    string? ImageFile,
    decimal Price
);

public sealed record CreateProductResponse(
    Guid Id
);

public sealed class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();
                
                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
}