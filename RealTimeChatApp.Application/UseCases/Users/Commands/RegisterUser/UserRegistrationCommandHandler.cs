using MediatR;
using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Services.Security;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.RegisterUser;

public class UserRegistrationCommandHandler : ICommandHandler<UserRegistrationCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserRegistrationCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        var salt = PasswordHelper.GenerateSalt();
        var password = PasswordHelper.HashPasswordWithSalt(request.password, salt);
        var user = new User
        {
            Username = request.username,
            Email = request.email,
            PasswordHash = password,
            PasswordSalt = Convert.ToBase64String(salt),
            PhoneNumber = request.phoneNumber
        };
        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();

    }
}
