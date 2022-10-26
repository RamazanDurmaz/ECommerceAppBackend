using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Jwt
{
    public class TokenOptions
    {
        /// <summary>
        /// Tokenın Kullanıcı Kitlesi
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// İmzalayan
        /// </summary>
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        /// <summary>
        /// Şifreleme Anahtarı
        /// </summary>
        public string SecurityKey { get; set; }

    }
}
