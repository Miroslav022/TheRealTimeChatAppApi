using RealTimeChatApp.Application.UseCases.Users.Commands.EditUser;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;

public interface IUserRepository
{
    void Add(User user);
    User GetByEmail(string email);
    Task<User> GetUserByIdAsync(string id);
    IEnumerable<User> GetUserByPhoneNumber(string PhoneNumber);

    Task<bool> UpdateAsync(EditUserCommand user);
    Task<bool> UpdateProfilePictureAsync(int id, string fileName);
    Task<bool> BlockUser(int userId, int blockedUserId);
    Task<bool> UnblockUser(int userId, int blockedUserId);
}
