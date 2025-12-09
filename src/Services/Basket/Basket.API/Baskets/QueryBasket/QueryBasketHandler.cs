
using Basket.API.Repositories;

namespace Basket.API.Baskets.QueryBasket;


public record QueryBasketCommand() : IRequest<QueryBasketResult>;


public record QueryBasketResult(IReadOnlyList<ShoppingCart> QueryBasketResultModel);


internal sealed class QueryBasketHandler(IBasketRepository basketRepository) : IRequestHandler<QueryBasketCommand, QueryBasketResult>
{
    public async Task<QueryBasketResult> Handle(QueryBasketCommand request, CancellationToken cancellationToken)
    {
        IReadOnlyList<ShoppingCart> query = await basketRepository.GetBasketList(cancellationToken);
        return new QueryBasketResult(query);
    }
}