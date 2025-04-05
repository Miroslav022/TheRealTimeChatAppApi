using RealTimeChatApp.Application.Abstractions.ValidationRules;
using System.Runtime.CompilerServices;

namespace RealTimeChatApp.Infrastructure.ValidationRules;

public class UserValidationRules : IUserValidationRules
{
    private readonly AspContext _aspContext;

    public UserValidationRules(AspContext aspContext)
    {
        _aspContext = aspContext;
    }

    public bool IsEmailTaken(string email)
    {
        return _aspContext.Users.Any(u => u.Email == email);
    }

    public bool IsPhoneNumberTaken(string phoneNumber)
    {
        return _aspContext.Users.Any(u=> u.PhoneNumber == phoneNumber);
    }
}
