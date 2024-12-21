using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersByCustomer;

public sealed class GetOrdersByCustomerQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerQueryResult>
{
    public async Task<GetOrdersByCustomerQueryResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId == CustomerId.Of(request.CustomerId))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrdersByCustomerQueryResult(orders.ToOrderDtoList());
    }
}