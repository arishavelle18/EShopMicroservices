
namespace Catalog.API.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            return Results.Ok(result);
        });
    }
}
