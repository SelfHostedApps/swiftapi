using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthCore.Services;

public class AuthService {
        //this allows to pass it as services in builder
        private readonly IConfiguration _config;
        
        public AuthService(IConfiguration config) {
                _config = config;
        }
        
        public string GenerateToken(Data.UserDto user) {
                //factory for tokens
                var handler = new JwtSecurityTokenHandler();
                
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
                
                //json token contains 3 sections
                //1 - Header: the token type and the algo used for encryption
                //2 - Payloads: content
                //3 - Signature: sign(header + payload + algo)
                var credentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                );
                
                var tokenDescriptor = new SecurityTokenDescriptor 
                {
                        Subject = GenerateClaims(user),//content
                        Expires = DateTime.UtcNow.AddMinutes(15),//lifespan
                        SigningCredentials = credentials,//the algo used
                        Issuer   = _config["Jwt:Issuer"],
                        Audience = _config["Jwt:Audience"],
                };

                var token = handler.CreateToken(tokenDescriptor);
                return handler.WriteToken(token);
        }


        private static ClaimsIdentity GenerateClaims(Data.UserDto user)
        {
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.Name, user.email));
                claims.AddClaim(new Claim(ClaimTypes.Role, user.role));

                return claims;
        }
}
