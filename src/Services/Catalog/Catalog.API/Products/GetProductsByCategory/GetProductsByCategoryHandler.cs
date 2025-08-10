using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryHandler(IDocumentSession session) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var getProductsByCategory = await session.Query<Product>().Where(item => item.Category.Contains(request.Category)).ToListAsync(cancellationToken);

        if (getProductsByCategory == null || !getProductsByCategory.Any())
        {
            throw new ProductNotFoundException("Products are not found!");
        }

        return new GetProductsByCategoryResult(getProductsByCategory);
    }
}
