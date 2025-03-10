using NeoMarket.Application.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoMarket.Application.Interfaces
{
    public interface IAccountService
    {
        UserDTO Authenticate(LoginRequest request);
    }

}
