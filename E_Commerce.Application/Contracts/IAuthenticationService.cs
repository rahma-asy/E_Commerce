using E_Commerce.Application.Comman;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    //no reposatory because i have Identity
    public interface IAuthenticationService
    {
        //Login=> take email & password and return email & token & displayName
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto,CancellationToken c=default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto,CancellationToken c=default);   
        Task<Result<bool>> CheckEmailExistsAsync(string email,CancellationToken c=default);
        Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken c = default);
        Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken c = default);
        Task<Result<AddressDto>> UpSertUserAddressAsync(string email, AddressDto address, CancellationToken c = default);
    }
}
