using IdentityTokenBasedAuth.Domain.Responses;
using IdentityTokenBasedAuth.Models;
using IdentityTokenBasedAuth.ResourceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Domain.Services
{
   public interface IUserService
    {
        //userviewmodel guncellenecek kullanicinin property olucak , username sahip kullanicinin bilgilerini userviewmodel deki degerlerle guncellenecek
        Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName);

        //username göre kullanici donecek
        Task<ApplicationUser> GetUserByUserName(string userName);

        Task<BaseResponse<ApplicationUser>> UploadUserPicture(string picturePath, string userName);

        //RefreshToken gore bir kullanicinin var olup olmadigini donecegiz

        //Refreshtoken son kullanma tarihini claim tablosunda tutuluyor 
        Task<Tuple<ApplicationUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken);

        //kullanici herhangi bir uygulamadan cikis yaptiginda refresh token silelim diye bu metodu yaziyoruz. kötü niyetli bir kullanici token ele gecirmesini engelliyoruz.
        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
