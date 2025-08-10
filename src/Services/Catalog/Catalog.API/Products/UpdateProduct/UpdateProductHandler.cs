
namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(2, 250).WithMessage("Product name must between 2 and 250 characters"); ;
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Product category is required.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.");
        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("Product image file is required.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}

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
