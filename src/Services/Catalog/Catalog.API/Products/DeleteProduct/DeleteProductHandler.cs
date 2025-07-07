namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(Guid Id);

public class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        //check the product if existing 
        var checkProductIsExisting = await session.Query<Product>()
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        if (checkProductIsExisting == null)
        {
            throw new Exception("Product not found");
        }
        session.Delete(checkProductIsExisting);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(checkProductIsExisting.Id);
    }
}

