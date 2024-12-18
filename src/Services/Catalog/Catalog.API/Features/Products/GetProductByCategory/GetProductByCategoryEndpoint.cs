using OpenTelemetry.Trace;

namespace Catalog.API.Features.Products.GetProductByCategory;

public sealed record GetProductByCategoryResponse(
    IEnumerable<Product> Products
);

public sealed class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                
                var response = result.Adapt<GetProductByCategoryResponse>();
                
                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
    }
}