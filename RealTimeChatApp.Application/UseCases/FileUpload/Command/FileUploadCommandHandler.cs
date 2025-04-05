using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.FileUpload.Command;

public sealed class FileUploadCommandHandler : ICommandHandler<FileUploadCommand, string>
{
    private readonly IFileUploadRepository _fileUploadRepository;

    public FileUploadCommandHandler(IFileUploadRepository fileUploadRepository)
    {
        _fileUploadRepository = fileUploadRepository;
    }

    public async Task<Result<string>> Handle(FileUploadCommand request, CancellationToken cancellationToken)
    {
        IFormFile file = request.File;
        Result<string> filePathResult = await _fileUploadRepository.Upload(file);
        return filePathResult;
    }
}
