using Basket.API.Data;

namespace Basket.API.Features.Basket.StoreBasket;

public sealed record StoreBasketCommand(ShoppingCart Cart)
    : ICommand<StoreBasketCommandResult>;

public sealed record StoreBasketCommandResult(
    ShoppingCart Cart    
);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public sealed class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketCommandResult>
{
    public async Task<StoreBasketCommandResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.StoreBasketAsync(request.Cart, cancellationToken);

        return new StoreBasketCommandResult(basket);
    }
}