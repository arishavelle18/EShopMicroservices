namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var getProduct = await session.Query<Product>().FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        if (getProduct == null)
        {
            throw new Exception("Product not found");
        }
        return new GetProductByIdResult(getProduct);
    }
}
