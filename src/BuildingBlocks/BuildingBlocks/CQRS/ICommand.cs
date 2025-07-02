using MediatR;

namespace BuildingBlocks.CQRS;

// Command interface for commands that return a response
public interface ICommand<out TResponse> : IRequest<TResponse>
{ 
}

// 
public interface ICommand : IRequest<Unit>
{
}