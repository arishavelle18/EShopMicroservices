using Basket.API.Repositories;
using Discount.Grpc.Protos;
using static Discount.Grpc.Protos.DiscountProtoService;

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

public class StoreBasketHandler(IDocumentSession session,IBasketRepository basketRepository,DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        //Deduct the discount from the basket items
        await DeductDiscount(request.ShoppingCart, cancellationToken);
        await basketRepository.StoreBasket(request.ShoppingCart, cancellationToken);
        return new StoreBasketResult(request.ShoppingCart.UserName);

    }

    public async Task DeductDiscount(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        foreach (var item in shoppingCart.ShoppingCartItems)
        {
            var coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
            item.Price -= coupon.Amount;
        }
    }
}
