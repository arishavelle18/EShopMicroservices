namespace Catalog.API.Products.QueryProduct;

public record QueryProductRequest(int? PageNumber = 1, int? PageSize = 10);
public record QueryProductResponse(IEnumerable<Product> Products);

public class QueryProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender, [AsParameters] QueryProductRequest queryProductRequest) =>
        {
            var req = queryProductRequest.Adapt<QueryProductQuery>();
            var result = await sender.Send(req);
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
