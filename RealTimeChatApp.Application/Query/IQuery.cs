using MediatR;

namespace RealTimeChatApp.Application.Query;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
