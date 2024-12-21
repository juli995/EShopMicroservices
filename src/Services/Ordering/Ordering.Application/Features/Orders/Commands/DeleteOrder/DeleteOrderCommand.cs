using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public sealed record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderCommandResult>;

public sealed record DeleteOrderCommandResult(bool IsSuccess);

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}