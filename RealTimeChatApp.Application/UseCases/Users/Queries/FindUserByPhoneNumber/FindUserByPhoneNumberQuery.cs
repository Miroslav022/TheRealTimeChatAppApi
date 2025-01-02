using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.UseCases.Users.DTOs;

namespace RealTimeChatApp.Application.UseCases.Users.Queries.FindUserByPhoneNumber;

public sealed record FindUserByPhoneNumberQuery(string PhoneNumber) : IQuery<IReadOnlyList<UserDto>>;
