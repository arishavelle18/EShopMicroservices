namespace Catalog.API.Products.CreateProduct;

//command
public record CreateProductCommand(string Name,
    List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;

//result
public record CreateProductResult(Guid Id);

// validator
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(2,250).WithMessage("Name must between 2 and 250 characters");
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

//handler
internal class CreateProductHandler(IDocumentSession session,ILogger<CreateProductHandler> logger, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Category = request.Category,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price
        };

        session.Store(newProduct);
        await session.SaveChangesAsync(cancellationToken);
        //return result
        return new CreateProductResult(newProduct.Id);
    }
}