using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Abstractions.Services;
using RealTimeChatApp.Domain.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace RealTimeChatApp.Application.UseCases.Messages.Command;

public class DeleteMessageCommandHandler : ICommandHandler<DeleteMessageCommand>
{
    private readonly IChatRepository _chatRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public DeleteMessageCommandHandler(IChatRepository chatRepository, IJwtTokenService jwtTokenService)
    {
        _chatRepository = chatRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        string access_token = request.Token;
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(access_token);
        if(principal.Error.Code != String.Empty && principal.Error.Description != String.Empty) return Result.Failure(principal.Error);
        var userIdClaim = principal.Value.FindFirst(JwtRegisteredClaimNames.Sub);
        
        if(int.TryParse(userIdClaim.Value, out int userId))
        {
            var result = await _chatRepository.DeleteMessage(request.Id, userId);
            if (!result) return Result.Failure(Error.Failure("Delete_message_failure", "Something went wrong"));
            return Result.Success();

        }
        else
        {
            return Result.Failure(Error.Failure("Delete_message_failure", "Something went wrong. Your claim is wrong"));
        }
        
    }
}
