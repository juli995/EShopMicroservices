using Marten.Schema;

namespace Basket.API.Models;

public sealed class ShoppingCart
{
    [Identity]
    public string UserName { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
    
    public ShoppingCart()
    { }
}