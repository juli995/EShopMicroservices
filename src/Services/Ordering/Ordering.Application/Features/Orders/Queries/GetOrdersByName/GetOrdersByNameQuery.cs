using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersByName;

public sealed record GetOrdersByNameQuery(string Name)
    : IQuery<GetOrdersByNameQueryResult>;
    
public sealed record GetOrdersByNameQueryResult(IEnumerable<OrderDto> Orders);