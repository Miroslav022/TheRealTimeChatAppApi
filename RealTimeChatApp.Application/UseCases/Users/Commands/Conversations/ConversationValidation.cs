using FluentValidation;
using RealTimeChatApp.Application.Abstractions.ValidationRules;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;

public class ConversationValidation : AbstractValidator<ConversationCommand>
{
    public ConversationValidation(IConversationValidationRules conversationValidation)
    {
        RuleFor(command => command.userId)
            .NotEmpty()
            .WithMessage("User id is required")
            .Must(conversationValidation.UserExists)
            .WithMessage("User id doesn't exist");

        RuleFor(command => command.createdBy)
            .NotEmpty()
            .WithMessage("createdBy id is required")
            .Must(conversationValidation.UserExists)
            .WithMessage("Createdby id doesn't exist");
    }
}
