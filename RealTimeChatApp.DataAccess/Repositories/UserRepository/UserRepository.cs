using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Services.Security;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Infrastructure.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly AspContext _context;

    public UserRepository(AspContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User GetByEmail(string email)
    {
        User user = _context.Users.FirstOrDefault(x => x.Email == email);
        return user;
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        User user = _context.Users.FirstOrDefault(x=>x.Id == int.Parse(id));
        return user;
    }
}
