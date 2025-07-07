
namespace Catalog.API.Products.GetProductsByCategory;

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(category));
            return Results.Ok(result);
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products by Category")
        .WithDescription("Get Products By Category");

    }
}
