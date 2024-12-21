using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersByName;

public sealed class GetOrdersByNameQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameQueryResult>
{
    public async Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(request.Name))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrdersByNameQueryResult(orders.ToOrderDtoList());
    }
}