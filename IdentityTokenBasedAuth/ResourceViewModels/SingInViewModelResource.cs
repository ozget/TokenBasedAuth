using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.ResourceViewModels
{
    public class SingInViewModelResource
    {//login olurken gerekli model
        [Required(ErrorMessage ="Email alanı gereklidir")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola alanı gereklidir")]
        [MinLength(4,ErrorMessage ="Parola en az 4 karakter olmalıdır")]
        public string Password { get; set; }
    }
}
