using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderCommandResult>;

public sealed record CreateOrderCommandResult(Guid Id);

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Order.CustomerId)
            .NotNull()
            .WithMessage("CustomerId is required");
        
        RuleFor(x => x.Order.OrderItems)
            .NotEmpty()
            .WithMessage("OrderItems is required");
    }
}