using IdentityTokenBasedAuth.Domain.Responses;
using IdentityTokenBasedAuth.ResourceViewModels;
using Microsoft.AspNetCore.Authentication.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Domain.Services
{
    public interface IAuthenticationService
    {
        Task<BaseResponse<UserViewModelResource>> SingUp(UserViewModelResource userViewModel);
        
        //Microsoft.AspNetCore.Authentication.Twitter
        Task<BaseResponse<AccessToken>> SingIn(SingInViewModelResource singInViewModel);

        Task<BaseResponse<AccessToken>> CreateTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);

        Task<BaseResponse<AccessToken>> RevokeResfreshToken(RefreshTokenViewModelResource refreshTokenViewModel);
    
    }
}
