﻿using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;

public interface IUserRepository
{
    void Add(User user);
    User GetByEmail(string email);
}
