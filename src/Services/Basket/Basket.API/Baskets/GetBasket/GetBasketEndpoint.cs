namespace Basket.API.Baskets.GetBasket;

public record GetBasketResponse(string UserName, List<ShoppingCartItem> ShoppingCartItems);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/baskets/{userName}", async (ISender sender, string userName, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(new GetBasketCommand(userName), cancellationToken);

            return Results.Ok(res);
        })
        .WithName("GetBasketByUserName")
        .Produces<GetBasketResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get basket by username")
        .WithDescription("Get basket by username");
    }
}
