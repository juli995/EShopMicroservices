namespace Basket.API.Exceptions;

public sealed class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string userName) : base ("Basket", userName)
    { }
}