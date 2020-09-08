using IdentityTokenBasedAuth.Domain.Responses;
using IdentityTokenBasedAuth.Domain.Services;
using IdentityTokenBasedAuth.Models;
using IdentityTokenBasedAuth.ResourceViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Services
{
    public class UserService:BaseService,IUserService
    {
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager):base (userManager, signInManager, roleManager)
        {

        }

        public async Task<Tuple<ApplicationUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken)
        {
            Claim claimRefreshToken = new Claim("refreshToken", refreshToken);

            
            // refresh token a gore var olan kullaniciyi donuyorum
            var users = await userManager.GetUsersForClaimAsync(claimRefreshToken);

            if (users.Any())
            {
                var user = users.First();
                IList<Claim> userClaims = await userManager.GetClaimsAsync(user);
                string refreshTokenEndDate = userClaims.First(c => c.Type == "refreshTokenEndDate").Value;
                if (DateTime.Parse(refreshTokenEndDate) > DateTime.Now)
                {// token omru dolmamis ise
                    return new Tuple<ApplicationUser, IList<Claim>>(user, userClaims);
                }
                else
                {
                    return new Tuple<ApplicationUser, IList<Claim>>(null, null);
                }

            }
            // refreshtoken sahip olmadigi icin null donuyoruz
            return new Tuple<ApplicationUser, IList<Claim>>(null, null);
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {// kullanici logout oldugunda

            var result = await GetUserByRefreshToken(refreshToken);
            if (result.Item1 == null) return false;

            IdentityResult response = await userManager.RemoveClaimsAsync(result.Item1, result.Item2);


            if (response.Succeeded)
            {
                return true;
            }
            return false; 
            
        }

        public async Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName)
        {
            ApplicationUser user = await userManager.FindByNameAsync(userName);

            if ((userManager.Users.Count(u => u.PhoneNumber == userViewModel.PhoneNumber)) > 1)
            {
                return new BaseResponse<UserViewModelResource>("Bu telefon numarası başka bir kullanıcıya ait");
            } // Tel no ile ilgili startup tarafinda bir property olmadigi icin if blogunu ile kontrol ettik


            user.BirthDay = userViewModel.BirthDay;
            user.City = userViewModel.City;
            user.Gender = (int)userViewModel.Gender;
            user.Email = userViewModel.Email; // Emaili startup tarafinda addIdentity icinde requireUniqueEmail bir kes olmadi diye belirtmis oluyoruz
            user.UserName = userViewModel.UserName;
            user.PhoneNumber = userViewModel.PhoneNumber;

            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)//update islemi basarisiz ise
            {//mapster viewmodel ile entity dönüşümü yapıyor
                return new BaseResponse<UserViewModelResource>(user.Adapt<UserViewModelResource>());
            }
            else
            {
                return new BaseResponse<UserViewModelResource>(result.Errors.First().Description);
            }



        }

        public async Task<BaseResponse<ApplicationUser>> UploadUserPicture(string picturePath, string userName)
        {

            ApplicationUser user = await userManager.FindByNameAsync(userName);
            user.Picture = picturePath;
            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new BaseResponse<ApplicationUser>(user);
            }
            else
            {
                return new BaseResponse<ApplicationUser>(result.Errors.First().Description);
            }


        }
    }
}
