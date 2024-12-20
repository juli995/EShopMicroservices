using Basket.API.Data;
using Discount.Grpc;

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

public sealed class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto,
    ILogger<StoreBasketCommandHandler> logger) : ICommandHandler<StoreBasketCommand, StoreBasketCommandResult>
{
    public async Task<StoreBasketCommandResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        await DeductDiscountAsync(request.Cart, cancellationToken);
        
        var basket = await repository.StoreBasketAsync(request.Cart, cancellationToken);

        return new StoreBasketCommandResult(basket);
    }

    private async Task DeductDiscountAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest
            {
                ProductName = item.ProductName
            }, cancellationToken: cancellationToken);
            
            item.Price -= coupon.Amount;
        }
    }
}