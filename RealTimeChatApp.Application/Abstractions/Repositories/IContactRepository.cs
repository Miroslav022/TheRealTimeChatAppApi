using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Application.Abstractions.Repositories;

public interface IContactRepository
{
    public void CreateContact(int userId, int contactUserId);
    public Task<List<Contact>> getContacts(int userId);
}
