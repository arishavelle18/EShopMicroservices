using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(string name, Guid id) : base(name,id)
    {
    }

    public ProductNotFoundException(string message = "Product is not found!") : base(message)
    {
    }
}
