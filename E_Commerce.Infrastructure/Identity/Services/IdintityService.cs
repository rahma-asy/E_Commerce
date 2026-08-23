using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class IdintityService : IIdintityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdintityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken c = default)
        {
            var user=await _userManager.FindByEmailAsync(email);
          
            if (user == null)
                return Result<bool>.Fail(Error.NotFound("User Not Found", $"User With Email {email} Not Foudddddd"));
            else
{
                var result =await _userManager.CheckPasswordAsync(user, password);
                return result;
}        }



        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken c = default)
        {
            var allUsers = _userManager.Users.ToList();
            Console.WriteLine($"Count = {allUsers.Count}");

            foreach (var u in allUsers)
            {
                Console.WriteLine($"{u.Email}");
            }

          
            var user = await _userManager.FindByEmailAsync(email);
            //  var user=await  _userManager.FindByEmailAsync(email);//will return application user
            if (user==null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User Not Found", $"User With Email {email} Not Found"));
            else
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id,user.DisplayName,user.Email,user.UserName));
        }
      
        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken c = default)
        {
            var user = new ApplicationUser()//will create it manual
            {
                Email = registerDto.Email, PhoneNumber = registerDto.PhoneNumber
                , UserName = registerDto.UserName, DisplayName = registerDto.DisplayName//password will sent for CreateAsync to hashing
            };

           var result=await _userManager.CreateAsync(user,"P@ssw0rd");//CreateAsync need application user

            return result.Succeeded ? Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id,user.DisplayName,user.Email,user.UserName))

                : Result<IdentityUserResult>.Fail(result.Errors.Select(x => new Error(x.Code, x.Description)).ToList());
                //we will protect[transelate] errors from IdentityUserResult to one of my errors
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRoles(string email, CancellationToken c = default)
        {
            var user =await _userManager.FindByEmailAsync(email);

            if (user == null) return Error.NotFound("User Not Found", "User With This Email IS Not Found");
            var rules=await _userManager.GetRolesAsync(user);
            return rules.ToList();
        }

        public async Task<Result<bool>> EmailExistsAsync(string email, CancellationToken c = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<Result<AddressDto>> GetUserAddressByEmailAsync(string email, CancellationToken c = default)
        {
            var user =await  _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x=>x.Email==email,c);
            if (user?.Address == null) return Result<AddressDto>.Fail(Error.NotFound("Address Not Found", "Address With This Email IS Not Found"));
            return new AddressDto()
            {
FirstName=user.Address.FirstName,LastName=user.Address.LastName,
                Street=user.Address.Street,City=user.Address.City,Country=user.Address.Country,
            };
        }

        public async Task<Result<AddressDto>> UpdateOrInserUserAddressAsync(string email, AddressDto address, CancellationToken c = default)
        {
            // no need to check if user is exist [if he can access to updateassrerss so he is authorize and have acount]
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, c);
            if (user?.Address == null) {
                //insert
                user.Address = new Address()
                {
                    FirstName = address.FirstName,
                    LastName  =address.LastName,
                    Street    = address.Street,
                    City      = address.City,
                    Country   = address.Country,
                };
}   
            else {
                //update

                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                   user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
               
            }

           var result=await _userManager.UpdateAsync(user);
            if (result.Succeeded) return address;
            else return Error.Failure("Failure", string.Join(';', result.Errors.Select(e => e.Description)));
            
        }
    }   
}
