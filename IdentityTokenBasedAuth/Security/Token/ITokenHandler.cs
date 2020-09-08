using IdentityTokenBasedAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Security.Token
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(ApplicationUser user);

        void RevokeRefreshToken(ApplicationUser user);
    }
}
