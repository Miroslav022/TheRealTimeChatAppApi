using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.BlockUser;

public sealed class BlockUserCommandHandler : ICommandHandler<BlockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public BlockUserCommandHandler(IUserRepository userRepositor)
    {
        _userRepository = userRepositor;
    }

    public async Task<Result> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        bool isUserAlreadyBlocked = await _userRepository.IsUserBlocked(request.userId, request.blockedUserId);
        if (isUserAlreadyBlocked) return Result.Success();

        var isAdded = await _userRepository.BlockUser(request.userId, request.blockedUserId);
        if (isAdded) return Result.Success();

        return Result.Failure(Error.Failure("Failed_blocking_user", "Something went wrong while blocking user"));
    }
}
