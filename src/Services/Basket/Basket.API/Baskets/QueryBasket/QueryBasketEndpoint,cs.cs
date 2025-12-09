
using Basket.API.Baskets.GetBasket;

namespace Basket.API.Baskets.QueryBasket;

public class QueryBasketEndpoint_cs : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/baskets/", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(new QueryBasketCommand(), cancellationToken);

            return Results.Ok(res);
        })
       .WithName("QueryBasket")
       .Produces<GetBasketResult>(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .WithSummary("Query All Basket")
       .WithDescription("Query All Basket");
    }
}
