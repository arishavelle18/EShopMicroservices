using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
    List<string> Catergory, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Guid);

internal class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //business logic for creating product
        //create product entity from command object
        var newProduct = new Product(Guid.CreateVersion7(), request.Name, request.Catergory, request.Description, request.ImageFile, request.Price);

        // TODO
        //save product to database

        //return result
        return new CreateProductResult(newProduct.Id);
    }
}