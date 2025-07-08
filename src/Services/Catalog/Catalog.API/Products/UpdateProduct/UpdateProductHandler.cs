
namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductHandler(IDocumentSession session,ILogger<UpdateProductHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var checkProductIsExisting = await session.Query<Product>()
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        if (checkProductIsExisting is null)
        {
            logger.LogError("Product not found!");
            throw new ProductNotFoundException();
        }
        // Update product entity from command object
        request.Adapt(checkProductIsExisting);
        session.Update(checkProductIsExisting);
        await session.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product updated successfully!");
        return new UpdateProductResult(true);
    }
}
