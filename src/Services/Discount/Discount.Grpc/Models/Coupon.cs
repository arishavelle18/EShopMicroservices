using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Discount.Grpc.Models;

public class Coupon
{
    public string Id { get; set; }
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }


    public static Coupon Create(string productName,string description, int amount)
    {
        return new Coupon
        {
            Id = Guid.NewGuid().ToString(),
            ProductName = productName,
            Description = description,
            Amount = amount
        };
    }

    public void Update(string productName, string description, int amount)
    {
        ProductName = productName;
        Description = description;
        Amount = amount;
    }
}
