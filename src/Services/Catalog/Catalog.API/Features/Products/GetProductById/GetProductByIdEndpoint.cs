namespace Catalog.API.Features.Products.GetProductById;

public sealed record GetProductByIdResponse(
    Product Product
);

public sealed class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                
                var response = result.Adapt<GetProductByIdResponse>();
                
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
    }
}