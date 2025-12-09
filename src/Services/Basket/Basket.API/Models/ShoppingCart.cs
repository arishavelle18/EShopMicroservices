using Marten.Schema;

namespace Basket.API.Models;

public class ShoppingCart
{
    [Identity]
    public string UserName { get; set; } = default!;

    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new();
    public decimal TotalPrice => ShoppingCartItems.Sum(item => item.Quantity * item.Price);
    


    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    public ShoppingCart()
    {
    }
}
