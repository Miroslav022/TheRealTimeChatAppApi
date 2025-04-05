using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Infrastructure.Repositories;

public class FileUploadRepository : IFileUploadRepository
{
    private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
    private readonly IWebHostEnvironment _environment;

    public FileUploadRepository(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<Result<string>> Upload(IFormFile file)
    {
        try
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            bool isValidExtension = allowedExtensions.Any(x => x == extension);
            if (!isValidExtension) return Result.Failure<string>(Error.Failure("unsupported_file_extension", "Wrong extension type. Allowed extensions are jpg, jpeg, png"));

            string uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string FileName = Guid.NewGuid().ToString() + extension;
            string FilePath = Path.Combine(uploadPath, FileName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Result.Success(FileName);
        }
        catch(Exception ex)
        {
            return Result.Failure<string>(Error.Failure("file_upload_failed", ex.Message));
        }
        
    }
}
