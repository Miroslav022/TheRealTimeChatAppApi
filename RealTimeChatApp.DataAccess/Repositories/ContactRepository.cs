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
        Contact contact = new Contact { UserId = userId, ContactUserId = contactUserId };
        _aspContext.Contacts.Add(contact);
        _aspContext.SaveChanges();
    }
     
    public async Task<List<Contact>> getContact(int userId)
    {
        return _aspContext.Contacts.Include(x=>x.ContactUser).Where(x=>x.UserId == userId).ToList();
    }
}
