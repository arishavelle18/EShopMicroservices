namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
    List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //business logic for creating product
        //create product entity from command object
        var newProduct = new Product
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Category = request.Category,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price
        };

        session.Store(newProduct);
        await session.SaveChangesAsync(cancellationToken);
        //return result
        return new CreateProductResult(newProduct.Id);
    }
}