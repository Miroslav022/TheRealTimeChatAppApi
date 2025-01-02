using MediatR;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.Abstractions.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}