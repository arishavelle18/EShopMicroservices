
namespace Catalog.API.Products.UpdateProduct;

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductModel request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateProductCommand(id,request));
            return Results.Ok(result);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Product")
        .WithDescription("Update Product");
    }
}
