namespace RealTimeChatApp.Application.Abstractions.ValidationRules;

public interface IUserValidationRules
{
    bool IsEmailTaken(string email);
    bool IsPhoneNumberTaken(string phoneNumber);
}
