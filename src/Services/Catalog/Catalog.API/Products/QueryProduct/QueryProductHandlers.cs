using Marten.Pagination;

namespace Catalog.API.Products.QueryProduct;

public record QueryProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<QueryProductResult>;
public record QueryProductResult(IEnumerable<Product> Products);

internal class QueryProductQueryHandlers(IDocumentSession session,ILogger<QueryProductQueryHandlers> logger) : IQueryHandler<QueryProductQuery, QueryProductResult>
{
    public async Task<QueryProductResult> Handle(QueryProductQuery request, CancellationToken cancellationToken)
    {
        var getAllProducts = await session.Query<Product>().ToPagedListAsync(request.PageNumber ?? 1, request.PageSize ?? 10, cancellationToken);
        logger.LogInformation("Retrieved {Count} products from the database", getAllProducts.Count);
        return new QueryProductResult(getAllProducts);
    }
}
