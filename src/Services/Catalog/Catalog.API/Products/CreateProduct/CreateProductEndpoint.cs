using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.CreateProduct;
public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResult>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product"); ;
    }
}
