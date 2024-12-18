using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Products.GetProducts;

public sealed record GetProductsRequest(
    int? PageNumber = 1,
    int? PageSize = 5
);

public sealed record GetProductsResponse(
    IEnumerable<Product> Products
);

public sealed class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                
                var result = await sender.Send(query);
                
                var response = result.Adapt<GetProductsResponse>();
                
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>()
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}