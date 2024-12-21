using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public sealed record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderCommandResult>;

public sealed record UpdateOrderCommandResult(bool IsSuccess);

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        
        RuleFor(x => x.Order.OrderName)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Order.CustomerId)
            .NotNull()
            .WithMessage("CustomerId is required");
    }
}