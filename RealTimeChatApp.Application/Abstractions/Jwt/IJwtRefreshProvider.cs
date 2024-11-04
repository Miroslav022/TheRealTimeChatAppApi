using RealTimeChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Abstractions.Jwt
{
    public interface IJwtRefreshProvider
    {
       public string Generate(User user);
    }
}
