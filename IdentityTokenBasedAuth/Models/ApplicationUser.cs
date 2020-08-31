﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string City { get; set; }
        public string Picture { get; set; }
        public DateTime? BirthDay { get; set; }

        public int Gender { get; set; }
    }
}
