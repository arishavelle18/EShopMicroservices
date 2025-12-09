
using Basket.API.Repositories;

namespace Basket.API.Baskets.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool Success);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username must required.");
    }
}

public class DeleteBasketHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        //Todo : Delete basket from database and cache
        // session.Delete<Product>(command.Id);
        var getBasket = await basketRepository.DeleteBasket(request.Username,cancellationToken);
        return new DeleteBasketResult(true);
    }
}
