using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
{
    public async Task<GetOrdersQueryResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.PaginationRequest.PageIndex;
        var pageSize = request.PaginationRequest.PageSize;
        
        var totalCount = await context.Orders.LongCountAsync(cancellationToken);
        
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .OrderBy(o => o.OrderName.Value)
            .Skip(request.PaginationRequest.PageIndex * request.PaginationRequest.PageSize)
            .Take(request.PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return new GetOrdersQueryResult(new PaginatedResult<OrderDto>
        (
            pageIndex,
            pageSize,
            totalCount,
            orders.ToOrderDtoList()
        ));
    }
}