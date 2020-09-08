using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTokenBasedAuth.Domain.Services;
using IdentityTokenBasedAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTokenBasedAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<ActionResult> getUser()
        {
            ApplicationUser user = await userService.GetUserByUserName(User.Identity.Name);
            

        }
    }
}
