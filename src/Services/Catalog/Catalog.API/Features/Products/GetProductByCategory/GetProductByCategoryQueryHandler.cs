namespace Catalog.API.Features.Products.GetProductByCategory;

public sealed record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryQueryResult>;

public sealed record GetProductByCategoryQueryResult(IEnumerable<Product> Products);

public sealed class GetProductByCategoryQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryQueryResult>
{
    public async Task<GetProductByCategoryQueryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryQueryResult(products);
    }
}