using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AspContext _aspContext;

    public ContactRepository(AspContext aspContext)
    {
        _aspContext = aspContext;
    }

    public void CreateContact(int userId, int contactUserId)
    {
        var contactExists = _aspContext.Contacts.Any(x => x.UserId == userId && x.ContactUserId == contactUserId);
        if (contactExists) return;

        Contact contact = new Contact { UserId = userId, ContactUserId = contactUserId };
        _aspContext.Contacts.Add(contact);
        _aspContext.SaveChanges();
    }
     
    public async Task<List<Contact>> getContacts(int userId)
    {
        return _aspContext.Contacts.Include(x=>x.ContactUser).Include(x=>x.User).Where(x=>x.UserId == userId).ToList();
    }
}
