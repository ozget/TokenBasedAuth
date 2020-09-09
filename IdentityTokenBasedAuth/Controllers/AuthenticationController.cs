using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTokenBasedAuth.Domain.Responses;
using IdentityTokenBasedAuth.Domain.Services;
using IdentityTokenBasedAuth.ResourceViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTokenBasedAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        public AuthenticationController(IAuthenticationService service)
        {
            this.authenticationService = service;
        }

        //bir kullanicini erisim izninin olup olmadigini belirlemek icin bir action
        public ActionResult IsAuthenticaiton()
        {
            // gecerli token ise true Sadece burasi token istiyor diger actionlar token istemiyor
            return Ok(User.Identity.IsAuthenticated);
        }

        [HttpPost] // kullanici kaydetme
        public async Task<ActionResult> SignUp(UserViewModelResource userViewModelResource)
        {
           BaseResponse<UserViewModelResource> response=  await authenticationService.SignUp(userViewModelResource);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }
        
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SingInViewModelResource signInViewModelResource)
        {
           var response = await authenticationService.SignIn(signInViewModelResource);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }

        }
        [HttpPost]
        public async Task<ActionResult> TokenByRefresToken(RefreshTokenViewModelResource refreshTokenViewModelResource)
        {
            var response = await authenticationService.CreateTokenByRefreshToken(refreshTokenViewModelResource);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }

        }
        [HttpDelete]// kullanici cikis yapildiğinda calisacaktir
        public async Task<ActionResult> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModelResource)
        {
            var response = await authenticationService.CreateTokenByRefreshToken(refreshTokenViewModelResource);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }

        }
    }
}
