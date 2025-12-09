namespace Basket.API.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string username) : base("Basket", username)
    {
    }
    public BasketNotFoundException(string name,string key) : base(name, key)
    {
    }
}