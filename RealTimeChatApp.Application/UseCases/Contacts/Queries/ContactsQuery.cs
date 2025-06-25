using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.UseCases.Contacts.DTOs;

namespace RealTimeChatApp.Application.UseCases.Contacts.Queries;

public sealed record ContactsQuery(int id) : IQuery<IReadOnlyCollection<ContactsDto>>
{
}
