using E_Commerce.Application.Comman;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IIdintityService
    {
        //find user by email and check if password is correct
        //Can NOT [Task<Result<ApplicationUser>>] Because ApplicationUser in Infrastructure
        //IdentityUserResult is the shape of obj that will be transfere from identityService(in Infrastructure Layer ) to Application Layer
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email,CancellationToken c=default);
        //we need email to know the user that i will check his password
        Task<Result<bool>> CheckPasswordAsync(string email, string password,CancellationToken c=default);
        //if he create user he will return IdentityUserResult
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken c = default);
      //needed in create token
        Task<Result<IReadOnlyList<string>>> GetUserRoles(string email, CancellationToken c = default);
        //end point of check if email is exist
        Task<Result<bool>> EmailExistsAsync(string email, CancellationToken c = default);
        Task<Result<AddressDto>>GetUserAddressByEmailAsync(string email, CancellationToken c = default);

        Task<Result<AddressDto>> UpdateOrInserUserAddressAsync(string email,AddressDto address, CancellationToken c = default);


    }
}
