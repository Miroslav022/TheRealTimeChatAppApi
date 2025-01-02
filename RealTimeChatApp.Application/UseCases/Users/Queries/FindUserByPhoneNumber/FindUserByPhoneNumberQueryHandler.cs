using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.UseCases.Users.DTOs;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Queries.FindUserByPhoneNumber;

public sealed class FindUserByPhoneNumberQueryHandler : IQueryHandler<FindUserByPhoneNumberQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public FindUserByPhoneNumberQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyList<UserDto>>> Handle(FindUserByPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = _userRepository.GetUserByPhoneNumber(request.PhoneNumber);
        
        if (users is null) {
            return Result.Failure<IReadOnlyList<UserDto>>(Error.NotFound("User.NotFound", $"The user with {request.PhoneNumber} was not Found"));
        }

        var response = users.Select(x => new UserDto ( x.Email, x.Username, x.Id, x.PhoneNumber, x.IsBlocked , x.ProfilePicture));

        return response.ToList();
    }
}
