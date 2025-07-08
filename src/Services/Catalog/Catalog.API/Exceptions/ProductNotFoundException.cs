using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid id) : base($"Product with id {id} not found!")
    {
    }

    public ProductNotFoundException() : base("Product is not found!")
    {
    }
}
