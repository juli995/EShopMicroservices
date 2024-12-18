using Marten.Pagination;

namespace Catalog.API.Features.Products.GetProducts;

public sealed record GetProductsQuery(
    int? PageNumber = 1,
    int? PageSize = 5
) : IQuery<GetProductsQueryResult>;

public sealed record GetProductsQueryResult(IEnumerable<Product> Products);

public sealed class GetProductsQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(
            query.PageNumber ?? 1,
            query.PageSize ?? 5,
            cancellationToken
        );

        return new GetProductsQueryResult(products);
    }
}