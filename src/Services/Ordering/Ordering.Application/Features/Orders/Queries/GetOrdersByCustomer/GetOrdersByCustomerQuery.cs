using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersByCustomer;

public sealed record GetOrdersByCustomerQuery(Guid CustomerId)
    : IQuery<GetOrdersByCustomerQueryResult>;
    
public sealed record GetOrdersByCustomerQueryResult(IEnumerable<OrderDto> Orders);