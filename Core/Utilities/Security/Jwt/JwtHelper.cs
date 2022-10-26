using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Core.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        //appsettings ten token bilgilerini okumak için IConfiguration kullanıyoruz
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        DateTime _accesTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            //appsettings ten nesne halinde çekiyoruz 
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _accesTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            //Bir algoritmayı kullanarak kendimize token oluşturucaz. O tokenı oluştururken(şifrelerken) yani encrypt ederken kendi bildiğimiz özel bir anahtara ihtiyacımız var.Securitykey bu yüzden kullanılıyor
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecuritKey)); -->Bu standart bir kod her tarafta kullaılabilir. Bu yüzdden Encryption dosyasında yaptık
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            //Json Web Token oluşturuluyor
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            //Jwt bir handler vasıtasıyla yazılıyor. WriteToken ile stringe çeviriyoruz
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accesTokenExpiration
            };
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            //notbefore anlamı tokenın expiration bilgisi şuandan önceyse geçerli değil
            return new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accesTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
                );
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            //jwt tokenı oluşturmak için gerekli olan claims alanı hazırlanıyor
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(x => x.Name).ToArray());
            return claims;

        }
    }
}
