using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Contacts.DTOs;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Contacts.Queries;

public class ContactQueryHandler : IQueryHandler<ContactsQuery, IReadOnlyCollection<ContactsDto>>
{
    private readonly IContactRepository _contactRepository;

    public ContactQueryHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Result<IReadOnlyCollection<ContactsDto>>> Handle(ContactsQuery request, CancellationToken cancellationToken)
    {
        List<Contact> contacts = await _contactRepository.getContacts(request.id);

        if (contacts is null)
        {
            return Result.Failure<IReadOnlyCollection<ContactsDto>>(Error.Failure("Contacts.NotFound", $"Request fail after getting the contacts for user with id - {request.id}"));
        }

        var response = contacts.Select(x => new ContactsDto(x.ContactUserId, x.ContactUser.Username, x.ContactUser.ProfilePicture, x.ContactUser.PhoneNumber)).ToList();

        return response;
    }
}
