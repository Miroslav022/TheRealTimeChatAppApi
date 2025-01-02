using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.UseCases.Messages.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Application.UseCases.Messages.Query;

public sealed record MessageQuery(int conversationId) : IQuery<List<MessagesDto>>
{
}
