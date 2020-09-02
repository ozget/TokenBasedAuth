using IdentityTokenBasedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Services
{
    public class BaseService:ControllerBase
    {
        //miras (implement eden) alanlar protected olanları kullanabilsin


        //kullanici islemlerini gerceklestiren clas userManagerdir.
        protected UserManager<ApplicationUser> userManager { get; }
        //signInManager kullanicinin login olmasi ile ilgili islemleri yapiyor
        protected SignInManager<ApplicationUser> signInManager { get; }

        protected RoleManager<ApplicationRole> roleManager { get; }

        public BaseService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole>roleManager)
        {
            // bunlar startup tarafinda addIdentity servisi sayesinde dolacaktir
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
    }
}
