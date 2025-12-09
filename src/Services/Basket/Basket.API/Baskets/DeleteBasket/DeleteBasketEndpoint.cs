
namespace Basket.API.Baskets.DeleteBasket;

public record DeleteBasketResponse(bool Success);
public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/baskets/{username}", async (string username, ISender sender, CancellationToken cancellationToken) =>
        {
            var req = await sender.Send(new DeleteBasketCommand(username), cancellationToken);
            var convertToDeleteResponse = req.Adapt<DeleteBasketResponse>();

            return Results.Ok(convertToDeleteResponse);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Basket by username")
        .WithDescription("Delete Basket by username");
    }
}
