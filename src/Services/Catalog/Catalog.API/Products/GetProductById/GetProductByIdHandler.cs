namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdHandler(IDocumentSession session, ILogger<GetProductByIdHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var getProduct = await session.LoadAsync<Product>(request.Id,cancellationToken);
        if (getProduct is null)
        {
            logger.LogError("Product is not found");
            throw new ProductNotFoundException();
        }
        logger.LogInformation($"{request.Id} is succeefully retrieved");
        return new GetProductByIdResult(getProduct);
    }
}
