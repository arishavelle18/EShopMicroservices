
namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, UpdateProductModel UpdateProductModel) : ICommand<UpdateProductResult>;

public record UpdateProductModel(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record UpdateProductResult(Guid Id);

internal class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var checkProductIsExisting = await session.Query<Product>()
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        if (checkProductIsExisting == null)
        {
            throw new Exception("Product not found");
        }
        // Update product entity from command object
        request.UpdateProductModel.Adapt(checkProductIsExisting);
        session.Store(checkProductIsExisting);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(checkProductIsExisting.Id);
    }
}
