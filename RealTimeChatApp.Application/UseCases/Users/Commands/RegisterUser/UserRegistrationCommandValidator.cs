using FluentValidation;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Abstractions.ValidationRules;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.RegisterUser;
public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
{
    public UserRegistrationCommandValidator(IUserValidationRules userValidationRules)
    {
        RuleFor(command => command.email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .Must(email => !userValidationRules.IsEmailTaken(email))
            .WithMessage("Email is already in use.");

        RuleFor(command => command.phoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Must(phoneNumber => !userValidationRules.IsPhoneNumberTaken(phoneNumber))
            .WithMessage("Phone number is already in use.");

        RuleFor(command => command.username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3)
            .WithMessage("Minimum length is 3 characters.");

        RuleFor(command => command.password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")
            .WithMessage("Password must be at least eight characters long and include at least one uppercase letter, one lowercase letter, and one number.");
    }
}
