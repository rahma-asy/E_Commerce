using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    //because we need ITokenService in IAuthenticationServic
    //ipmlementation will be in Infrastructure  because it will use external services
    //we cann't do is as a helper method in AuthenticationServic because it will use external services to generate token
    public interface ITokenService
    {
        string CreateToken(string userId,string email,string userName,IReadOnlyList<string>roles);
    }
}
