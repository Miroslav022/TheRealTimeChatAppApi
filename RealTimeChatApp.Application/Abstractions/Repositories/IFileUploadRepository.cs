using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.Abstractions.Repositories;

public interface IFileUploadRepository
{
    Task<Result<string>> Upload(IFormFile file);
}
