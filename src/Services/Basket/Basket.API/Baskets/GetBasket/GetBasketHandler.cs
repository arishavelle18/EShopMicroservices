using Basket.API.Repositories;
using FluentValidation;

namespace Basket.API.Baskets.GetBasket;

public record GetBasketCommand(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(GetBasketResultModel ShoppingCart);
public record GetBasketResultModel(string UserName, List<ShoppingCartItem> ShoppingCartItems, decimal TotalPrice);

public class GetBasketCommandValidator : AbstractValidator<GetBasketCommand>
{
    public GetBasketCommandValidator()
    {
       RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} must be required");
    }
}

public class GetBasketHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketCommand, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketCommand request, CancellationToken cancellationToken)
    {
        //TODO: get basket from database
        // var basket = await _repository.GetBasket(request.UserName, cancellationToken);

        var getBasket = await basketRepository.GetBasket(request.UserName,cancellationToken);

        if (getBasket == null)
        {
            throw new BasketNotFoundException(nameof(ShoppingCart), request.UserName);
        }
        return new GetBasketResult(new GetBasketResultModel(getBasket.UserName,getBasket.ShoppingCartItems,getBasket.TotalPrice));
    }
}
