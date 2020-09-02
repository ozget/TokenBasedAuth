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

        public Task<Tuple<ApplicationUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public Task<bool> RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
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

        public Task<BaseResponse<ApplicationUser>> UploadUserPicture(string picturePath, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
