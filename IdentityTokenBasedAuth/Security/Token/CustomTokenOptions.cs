using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Security.Token
{
    public class CustomTokenOptions
    {
        public string Audience { get; set; }//dinleyici
        public string Issuer { get; set; }//dağıtıcı
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenexpiration { get; set; } //tokenın ömürü ile ilgili
        public string SecuntyKey { get; set; }
    }
}
