using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Services.Security;
using RealTimeChatApp.Application.UseCases.Users.Commands.EditUser;
using RealTimeChatApp.Domain.Entities;

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

    public async Task<bool> BlockUser(int userId, int blockedUserId)
    {
        BlockedContact blockedContact = new BlockedContact { BlockedUserId = blockedUserId , UserId = userId};
        _context.BlockedContacts.Add(blockedContact);
        var isSuccess = await _context.SaveChangesAsync();
        if(isSuccess > 0) return true;
        return false;

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

    public IEnumerable<User> GetUserByPhoneNumber(string PhoneNumber)
    {
        IEnumerable<User> users = _context.Users.Where(x=>x.PhoneNumber.Contains(PhoneNumber));
        return users;
    }

    public async Task<bool> UnblockUser(int userId, int blockedUserId)
    {
        var unblockUser = _context.BlockedContacts.FirstOrDefault(x=> x.UserId == userId && x.BlockedUserId == blockedUserId);
        _context.BlockedContacts.Remove(unblockUser);
        var result = await _context.SaveChangesAsync();
        if (result > 0) return true;
        return false;
    }

    public async Task<bool> UpdateAsync(EditUserCommand user)
    {
        User userToEdit = await GetUserByIdAsync(user.id.ToString());
        if(userToEdit is null) return false;

        userToEdit.Username = string.IsNullOrWhiteSpace(user.username) ? userToEdit.Username : user.username;
        userToEdit.Email = string.IsNullOrWhiteSpace(user.email) ? userToEdit.Email : user.email;

        byte[] sald = PasswordHelper.GenerateSalt();
        userToEdit.PasswordHash = string.IsNullOrWhiteSpace(user.password) ? userToEdit.PasswordHash : PasswordHelper.HashPasswordWithSalt(user.password, sald);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProfilePictureAsync(int id, string fileName)
    {
        User user = await GetUserByIdAsync(id.ToString());
        if(user is null) return false;

        user.ProfilePicture = fileName;
        await _context.SaveChangesAsync();
        return true;
    }
}
