using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.System.Users;

namespace Application.Common.System.Users
{
    public interface IUserService
    {
        Task<string> Authenticated(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
    }
}
