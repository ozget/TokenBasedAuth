using IdentityTokenBasedAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Security.Token
{
    interface ITokenHandler
    {
        AccesToken CreateAccessToken(ApplicationUser user);

        void RevokeRefreshToken(ApplicationUser user);
    }
}
