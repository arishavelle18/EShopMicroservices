namespace Catalog.API.Products.QueryProduct;

public record QueryProductQuery() : IQuery<QueryProductResult>;
public record QueryProductResult(IEnumerable<Product> Products);

internal class QueryProductHandlers(IDocumentSession session) : IQueryHandler<QueryProductQuery, QueryProductResult>
{
    public async Task<QueryProductResult> Handle(QueryProductQuery request, CancellationToken cancellationToken)
    {
        var getAllProducts = await session.Query<Product>().ToListAsync(cancellationToken);

        return new QueryProductResult(getAllProducts);
    }
}
