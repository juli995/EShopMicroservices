using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public sealed class DeleteOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteOrderCommand, DeleteOrderCommandResult>
{
    public async Task<DeleteOrderCommandResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.OrderId);

        var order = await context.Orders
            .FindAsync([orderId], cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        context.Orders.Remove(order);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return new DeleteOrderCommandResult(true);
    }
}