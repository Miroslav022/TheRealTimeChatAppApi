using RealTimeChatApp.Application.Abstractions.ValidationRules;

namespace RealTimeChatApp.Infrastructure.ValidationRules;

public class ConversationValidationRules : IConversationValidationRules
{
    private readonly AspContext _aspContext;

    public ConversationValidationRules(AspContext aspContext)
    {
        _aspContext = aspContext;
    }

    public bool UserExists(int userId)
    {
        return _aspContext.Users.Any(x=>x.Id == userId);
    }
}
