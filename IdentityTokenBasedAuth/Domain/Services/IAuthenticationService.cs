using IdentityTokenBasedAuth.Domain.Responses;
using IdentityTokenBasedAuth.ResourceViewModels;
using IdentityTokenBasedAuth.Security.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Domain.Services
{
    public interface IAuthenticationService
    {
        Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel);
        
        Task<BaseResponse<AccessToken>> SignIn(SingInViewModelResource singInViewModel);

        Task<BaseResponse<AccessToken>> CreateTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);

        Task<BaseResponse<AccessToken>> RevokeResfreshToken(RefreshTokenViewModelResource refreshTokenViewModel);
    
    }
}
