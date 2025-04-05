using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.UnblockUser;

public sealed class UnblockUserCommandHandler : ICommandHandler<UnblockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UnblockUserCommandHandler(IUserRepository userRepositor)
    {
        _userRepository = userRepositor;
    }

    public async Task<Result> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
    {
        var isAdded = await _userRepository.UnblockUser(request.userId, request.blockedUserId);
        if (isAdded) return Result.Success();
        return Result.Failure(Error.Failure("Failed_unblocking_user", "Something went wrong while unblocking user"));
    }
}
