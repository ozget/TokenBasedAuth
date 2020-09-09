using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityTokenBasedAuth.Domain.Services;
using IdentityTokenBasedAuth.Models;
using IdentityTokenBasedAuth.ResourceViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityTokenBasedAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize] // bir token bekledigini belitrmis oluyoruz
    public class UserController : ControllerBase,IActionFilter
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> getUser()
        {
            ApplicationUser user = await userService.GetUserByUserName(User.Identity.Name);

            return Ok(user.Adapt<UserViewModelResource>());

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {// metodlardan biri calismasi bittikten sonra birseyler yapilabilir
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {//metodlardan herhangi biri calisirken herhangi birsey yapabilirim
         
            // metoda gitmeden required password sil gormezden gel diyoruz
            context.ModelState.Remove("Password");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserViewModelResource userViewModelResource)
        { // sifreyi guncellemicek
            var response = await userService.UpdateUser(userViewModelResource,User.Identity.Name);
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
        public async Task<IActionResult> UpdateUserPicture(IFormFile picture)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory() + "wwwroot/UserPictures",fileName);

            using(var stream= new FileStream(path, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }

            var result = new
            {
                path = "https://" + Request.Host + "/UserPictures" + fileName
            };
            var response = await userService.UploadUserPicture(result.path, User.Identity.Name);

            if (response.Success)
            {// isimsiz classi json a donusturecek client ulastiracak
                return Ok(path);
            }
            else
            {
                return BadRequest(response.Message);
            }
        
        }
    }
}
