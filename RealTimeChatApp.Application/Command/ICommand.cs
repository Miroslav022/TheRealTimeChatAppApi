using MediatR;

namespace RealTimeChatApp.Application.Command;

public interface ICommand : IRequest
{
}

public interface ICommand<TResponse> : IRequest<TResponse> { }