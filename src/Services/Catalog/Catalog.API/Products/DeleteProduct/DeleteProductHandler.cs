namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        //check the product if existing 
        var checkProductIsExisting = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (checkProductIsExisting is null)
        {
            logger.LogError("Product not Found!");
            throw new ProductNotFoundException();
        }
        session.Delete(checkProductIsExisting);
        await session.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product deleted succefully!");
        return new DeleteProductResult(true);
    }
}

