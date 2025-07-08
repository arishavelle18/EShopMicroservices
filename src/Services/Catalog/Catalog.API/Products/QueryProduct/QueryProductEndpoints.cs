namespace Catalog.API.Products.QueryProduct;

public record QueryProductResponse(IEnumerable<Product> Products);

public class QueryProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new QueryProductQuery());
            var convertToQueryProductResponse = result.Adapt<QueryProductResponse>();
            return Results.Ok(convertToQueryProductResponse);
        })
        .WithName("GetAllProducts")
        .Produces<QueryProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Products")
        .WithDescription("Get All Products");
    }
}
