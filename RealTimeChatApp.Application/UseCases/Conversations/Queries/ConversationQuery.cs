using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;

namespace RealTimeChatApp.Application.UseCases.Conversations.Queries;

public sealed record ConversationQuery(int id) : IQuery<IReadOnlyList<ConversationDto>>
{
}
