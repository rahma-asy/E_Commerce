using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JWTSettings  _jwtoptions;

        public TokenService(IOptions<JWTSettings> jwtoptions) 
        {
            _jwtoptions = jwtoptions.Value;
        }
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            #region clims
            var clims = new List<Claim>()
            {
                 new Claim(ClaimTypes.NameIdentifier,userId) ,  //have  type and value
                  new Claim(ClaimTypes.Email,email) ,
                   new Claim(ClaimTypes.Name,userName) ,
            };
            clims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            #endregion

            #region signingCredentials
            //signingCredentials: algorithm &  secret key 
            //Sensitive and changeable
            var secKey = _jwtoptions.SecretKey;

            if (string.IsNullOrEmpty(secKey))
                throw new InvalidOperationException("JWT SecretKey Is Missing ");

            if (secKey.Length<20)
                throw new InvalidOperationException("JWT SecretKey Is Too Short ");
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey)); //transelate to suitable Bytes
            Console.WriteLine(secKey);
            Console.WriteLine(secKey.Length);
            Console.WriteLine(Encoding.UTF8.GetBytes(secKey).Length);
            var credentials=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            #endregion

            //issuer ,audience , expires => will get it from _jwtoptions

            var token = new JwtSecurityToken(//all data

                issuer: _jwtoptions.Issuer,// the server which create token 
                audience:_jwtoptions.Audience,// which URL Receives and use this token[ application or api or frontand ]
                claims: clims,//extra information aboute user[ userId, email, userName,roles]
                expires:DateTime.UtcNow.AddMinutes( _jwtoptions.ExpirationMinutes),//when token will be invalid//after expires user should login to create new token
                signingCredentials: credentials//must be in token to know algorithm we will use in sign & what is secret key that will used to validate[ensure that you are same sign(no edits)]
                );

           return new JwtSecurityTokenHandler().WriteToken(token); //need obj of SecurityToken that have all data to encoded string[belods & signeture  & header]
       //return string
        }
    }

    //we want IOptions to get url from Appsettings [obj منه ويحطها هو  configurations بحدد المكان وهو بيقرا ال ]
    public class JWTSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public int ExpirationMinutes { get; set; }
        public string Audience { get; set; } = default!;

    }
}
