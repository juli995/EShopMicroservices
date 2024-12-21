using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Ordering.Application.Dtos;

namespace Ordering.Application.Features.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersQueryResult>;
    
public sealed record GetOrdersQueryResult(PaginatedResult<OrderDto> Orders);