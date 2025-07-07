
namespace Catalog.API.Products.GetProductById;

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdQuery>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product by Id")
        .WithDescription("Get Product By Id");
    }
}
