namespace RealTimeChatApp.Application.Abstractions.ValidationRules;

public interface IConversationValidationRules
{
    bool UserExists(int userId);
}
