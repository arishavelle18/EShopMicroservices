using Basket.API.Repositories;

namespace Basket.API.Baskets.StoreBasket;

public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName)
            .NotEmpty().WithMessage("User name is required.");
        RuleFor(x => x.ShoppingCart)
            .NotNull().WithMessage("Shopping cart must not null");
    }
}

public class StoreBasketHandler(IDocumentSession session,IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        // ShoppingCart cart = request.ShoppingCart;
        //TODO:  store basket in database (use marten upsert - if exists update, if not insert)
        //TODO:  update cache
        //check if exist
        await basketRepository.StoreBasket(request.ShoppingCart, cancellationToken);
        return new StoreBasketResult(request.ShoppingCart.UserName);

    }
}
