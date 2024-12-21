using BuildingBlocks.CQRS;
using Mapster;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderCommandResult>
{
    public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(request.Order);

        context.Orders.Add(order);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return new CreateOrderCommandResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(
            orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.EmailAddress,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode
        );
        
        var billingAddress = Address.Of(
            orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.EmailAddress,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode
        );

        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod
            ),
            status: OrderStatus.Pending
        );

        foreach (var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }
        return newOrder;
    }
}