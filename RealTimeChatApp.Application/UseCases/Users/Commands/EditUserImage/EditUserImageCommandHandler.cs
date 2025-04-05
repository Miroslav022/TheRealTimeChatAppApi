using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.UseCases.FileUpload.Command;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.EditUserImage;

public sealed class EditUserImageCommandHandler : ICommandHandler<EditUserImageCommand>
{
    private readonly IFileUploadRepository _fileUploadRepository;
    private readonly IUserRepository _userRepository;

    public EditUserImageCommandHandler(IFileUploadRepository fileUploadRepository, IUserRepository userRepository)
    {
        _fileUploadRepository = fileUploadRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(EditUserImageCommand request, CancellationToken cancellationToken)
    {
        var fileUploadCommand = new FileUploadCommand
        {
            File = request.File
        };
        var fileName = new FileUploadCommandHandler(_fileUploadRepository).Handle(fileUploadCommand, cancellationToken);
        bool isSuccess = await _userRepository.UpdateProfilePictureAsync(request.Id, fileName.Result.Value);
        
        if(!isSuccess) return Result.Failure(Error.Failure("failed", "updating failed"));

        return Result.Success();
    }
}
