using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    //this is business layer[checking] not linked by which framework you use & no need for know you will use user manger or ant thing else 
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdintityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdintityService identityService,ITokenService tokenService )
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }


        //
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken c = default)
        {
            //To login :- get user by email , check if password is correct , return userDto or result
            //to do that i need user manger [be in infrastructure] so we doing a contruct That will implamented in infrastructure
            var userResult=await _identityService.FindUserByEmailAsync(loginDto.Email);
            if (!userResult.IsSuccess) return Result<UserDto>.Fail(userResult.Errors) ;

            //check if password is correct 
            var passwordResult = await _identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password);
            if (!passwordResult.IsSuccess) return Result<UserDto>.Fail(userResult.Errors); // IsSuccess but wrong password[Success<false>]
              
            if (!passwordResult.Value) return Result<UserDto>.Fail(Error.Unauthorized("InValid Email Or Password"));

            #region session 6
            var user = userResult.Value;
            var rules =await _identityService.GetUserRoles(user.Email);
            var token = _tokenService.CreateToken(user.Id,user.Email,user.UserName,rules.Value);
            #endregion
            return new UserDto()
            {
                Email = loginDto.Email,
                DisplayName = userResult.Value.DisplayName,
                Token =token
            };

        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken c = default)
        {
            var result=await _identityService.CreateUserAsync(registerDto,c);
            if (!result.IsSuccess) return Result<UserDto>.Fail(result.Errors);

            //return new UserDto()
            //{
            //    Email = registerDto.Email,
            //    DisplayName = registerDto.DisplayName,
            //    Token = "Token"
            //};

            #region session 6
            var user = result.Value;
            var rules = await _identityService.GetUserRoles(user.Email);
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, rules.Value);
            #endregion
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                Token =token
            });
        }

        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken c = default)
        =>await _identityService.EmailExistsAsync(email,c);

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken c = default)
        {
            var userDto=await _identityService.FindUserByEmailAsync(email,c);
         
            var user = userDto.Value;
            var rules = await _identityService.GetUserRoles(user.Email);
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, rules.Value);
       return new UserDto() { DisplayName=user.DisplayName, Token=token,Email=email };
        
        }

        public async Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken c = default)
        =>await _identityService.GetUserAddressByEmailAsync(email,c);

        public async Task<Result<AddressDto>> UpSertUserAddressAsync(string email, AddressDto address, CancellationToken c = default)
       =>await _identityService.UpdateOrInserUserAddressAsync(email, address, c);
    }
}
