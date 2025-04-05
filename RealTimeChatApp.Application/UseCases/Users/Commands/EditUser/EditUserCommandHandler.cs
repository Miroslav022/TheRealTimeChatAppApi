using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.EditUser;

public class EditUserCommandHandler : ICommandHandler<EditUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        bool isUpdated = await _userRepository.UpdateAsync(request);
        if(isUpdated) return Result.Success();
        return Result.Failure(Error.Failure("User_Edit_Failed", "Something went wrong during editing"));
    }
}
