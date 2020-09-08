using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityTokenBasedAuth.Domain.Responses;
using IdentityTokenBasedAuth.Domain.Services;
using IdentityTokenBasedAuth.Models;
using IdentityTokenBasedAuth.ResourceViewModels;
using Microsoft.AspNetCore.Identity;
using IdentityTokenBasedAuth.Security.Token;
using Microsoft.Extensions.Options;
using Mapster;

namespace IdentityTokenBasedAuth.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly ITokenHandler tokenHandler;
        private readonly CustomTokenOptions tokenOptions;
        private readonly IUserService userService;


        public AuthenticationService(UserManager<ApplicationUser> userManager, IUserService userService, ITokenHandler tokenHandler, IOptions<CustomTokenOptions> tokenOptions, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager) : base(userManager, signInManager, roleManager)
        {
            this.tokenHandler = tokenHandler;
            this.userService = userService;
            this.tokenOptions = tokenOptions.Value;
        }

        public async Task<BaseResponse<AccessToken>> CreateTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {
            var userClaim = await userService.GetUserByRefreshToken(refreshTokenViewModel.RefreshToken);

            if (userClaim.Item1 != null)
            {
                AccessToken accessToken = tokenHandler.CreateAccessToken(userClaim.Item1);
                Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);

                Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(tokenOptions.RefreshTokenexpiration).ToString());
                //userClaim.Item2[0] refrestoken karsilik gelir
                //userClaim.Item2[1] refrestokenEndDateCliam karsilik geliyor
                //userClaim.Item1 applicationUser barindiriyor, userClaim.Item2[0] refresToken demektir
                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[0], refreshTokenClaim);
                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[1], refreshTokenEndDateClaim);

                return new BaseResponse<AccessToken>(accessToken);

            }
            else
            {
                //kullanici Cikis İslemi gerceklestirmis olabilir
                return new BaseResponse<AccessToken>("Böyle bir refreshtoken sahip kullanıcı yok");
            }
        } 

        public async Task<BaseResponse<AccessToken>> RevokeResfreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {
            bool result = await userService.RevokeRefreshToken(refreshTokenViewModel.RefreshToken);
            if (result)
            {
                return new BaseResponse<AccessToken>(new AccessToken());
            }
            else
            {
                return new BaseResponse<AccessToken>("refreshToken veritabanından bulunmuyor");
            }
        
        
        }

        public async Task<BaseResponse<AccessToken>> SingIn(SingInViewModelResource singInViewModel)
        {
            // bir nevi login islemi
            ApplicationUser user = await userManager.FindByEmailAsync(singInViewModel.Email);
            if (user != null)
            {
                bool IsUser = await userManager.CheckPasswordAsync(user, singInViewModel.Password);


                if (IsUser)
                {
                    AccessToken accessToken = tokenHandler.CreateAccessToken(user);

                    Claim refreshTokenClaim = new Claim("refreshToken",accessToken.RefreshToken);

                    Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(tokenOptions.RefreshTokenexpiration).ToString());

                    //daha once kullanicin refreshtokin varmi kontrol edecegiz
                    List<Claim> refreshClaimList = userManager.GetClaimsAsync(user).Result.Where(c => c.Type.Contains("refreshToken")).ToList();

                    if (refreshClaimList.Any())
                    {
                        //(user,degistirilecek,yeni olanı kaydetme refresTokenClaim)
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[0], refreshTokenClaim);
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[1], refreshTokenClaim);

                    }
                    else
                    {
                        await userManager.AddClaimsAsync(user,new[] { refreshTokenClaim, refreshTokenEndDateClaim });

                    }
                    return new BaseResponse<AccessToken>(accessToken);
                }

                return new BaseResponse<AccessToken>("şifre yanlış");
            }

            return new BaseResponse<AccessToken>("Email yanlış");
        }

        public async Task<BaseResponse<UserViewModelResource>> SingUp(UserViewModelResource userViewModel)
        {//kullanici kayit etme
            ApplicationUser user = new ApplicationUser
            {
                UserName=userViewModel.UserName,
                Email=userViewModel.Email,

            };
            IdentityResult result = await userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return new BaseResponse<UserViewModelResource>(user.Adapt<UserViewModelResource>());

            }
            else
            {
                return new BaseResponse<UserViewModelResource>(result.Errors.First().Description);
            }
        }
    }
}
