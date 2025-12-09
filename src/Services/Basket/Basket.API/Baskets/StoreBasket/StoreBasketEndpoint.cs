
namespace Basket.API.Baskets.StoreBasket;

public record StoreBasketRequest(ShoppingCart ShoppingCart);
public record StoreBasketResponse(string Username);
public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets",async(StoreBasketRequest storeBasketRequest, ISender sender, CancellationToken cancellationToken) =>
        {
            var convertToStoreBasket = storeBasketRequest.Adapt<StoreBasketCommand>();
            var shoppingCart = await sender.Send(convertToStoreBasket, cancellationToken);
            return Results.Ok(shoppingCart);
        })
        .WithName("StoreBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Store and Update Basket")
        .WithDescription("Store and Update Basket");
    }
}
