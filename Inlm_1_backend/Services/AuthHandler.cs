using Inlm_1_backend.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;



namespace Inlm_1_backend.Services
{
    public class AuthHandler
    {
        private readonly IConfiguration _configuration;
        

        public AuthHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret_key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds

                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        public bool ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret_key").Value));
            var jwt = new JwtSecurityTokenHandler();
            try
            {
            jwt.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = key
            }, out SecurityToken validatedToken);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        } 

        public int GetIdFromToken(string token)
        {
            try
            {
            var _token = new JwtSecurityToken(jwtEncodedString: token);
            var _id = _token.Claims.FirstOrDefault(x => x.Type == "id")!.Value;


            return int.Parse(_id);
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
            
        }
    }
}
