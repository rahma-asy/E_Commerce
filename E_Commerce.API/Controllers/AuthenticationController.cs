using E_Commerce.Application.Comman;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService  )
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login,CancellationToken c)
                  =>  ToActionResult(await _authenticationService.LoginAsync(login,c));

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto ,CancellationToken c)
          => ToActionResult(await _authenticationService.RegisterAsync(registerDto,c));//will return depend on response is ok or fail

        //check if email exist
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery]string email,CancellationToken c)
        => ToActionResult(await _authenticationService.CheckEmailExistsAsync(email,c));
        //get current user

        //if he want to get current user he will sent email[you are authorize] =>
        //you are authorize then you have part of token clims is email
        [HttpGet("currentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser( CancellationToken c)//he will not send email he is already log in
        {
            //email is exist
            var email=User.FindFirstValue(ClaimTypes.Email);
            if (email == null) throw new UnauthorizedAccessException("No Email Clim Found");
            else return ToActionResult(await _authenticationService.GetCurrentUserAsync(email, c)); 
            
        }

        //get current user address
        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress(CancellationToken c)
        {

            //var email = User.FindFirstValue(ClaimTypes.Email);
            //if (email == null) throw new UnauthorizedAccessException("No Email Clim Found");   or use GetEmailFromToken as a helper method

         return ToActionResult(await _authenticationService.GetUserAddressAsync(GetEmailFromToken(), c));

        }
        //update current user address
        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address,CancellationToken c)
        {

            return ToActionResult(await _authenticationService.UpSertUserAddressAsync(GetEmailFromToken(),address, c));

        }
    }
}
