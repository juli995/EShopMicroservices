using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using Discount.Grpc;
using MassTransit;

namespace Basket.API.Features.Basket.CheckoutBasket;

public sealed record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    : ICommand<CheckoutBasketCommandResult>;

public sealed record CheckoutBasketCommandResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto cannot be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotNull().WithMessage("UserName cannot be null");
    }
}

public sealed class CheckoutBasketCommandHandler(
    IBasketRepository repository,
    IPublishEndpoint publishEndpoint,
    ILogger<CheckoutBasketCommandHandler> logger) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketCommandResult>
{
    public async Task<CheckoutBasketCommandResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);

        if (basket is null)
        {
            return new CheckoutBasketCommandResult(false);
        }
        
        var eventMessage = request.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        
        await repository.DeleteBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);
        
        return new CheckoutBasketCommandResult(true);
    }
}