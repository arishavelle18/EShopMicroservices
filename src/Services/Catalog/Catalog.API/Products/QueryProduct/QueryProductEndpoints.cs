namespace Catalog.API.Products.QueryProduct;

public class QueryProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new QueryProductQuery());

            return Results.Ok(result);
        })
        .WithName("GetAllProducts")
        .Produces<QueryProductResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Products")
        .WithDescription("Get All Products");
    }
}
