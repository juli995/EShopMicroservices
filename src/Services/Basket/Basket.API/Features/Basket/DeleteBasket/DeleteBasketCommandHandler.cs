using Basket.API.Data;

namespace Basket.API.Features.Basket.DeleteBasket;

public sealed record DeleteBasketCommand(string UserName)
    : ICommand<DeleteBasketCommandResult>;

public sealed record DeleteBasketCommandResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public sealed class DeleteBasketCommandHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketCommandResult>
{
    public async Task<DeleteBasketCommandResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteBasketAsync(request.UserName, cancellationToken);

        return new DeleteBasketCommandResult(true);
    }
}