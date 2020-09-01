using IdentityTokenBasedAuth.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.ResourceViewModels
{//Kullanici kaydedecegimiz model, addPointime yeni bir kullanici kaydi geldigi zaman tutacak modeldir
    public class UserViewModelResource
    {
        public string UserName { get; set; }
        [RegularExpression(@"^(0(\d{3}-)(\d{3}-)(\d{2})(\d{2}))$",ErrorMessage ="Telefon numarası uygun formatta değildir")]
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage ="Email Adresiniz doğru formatta değil")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Picture { get; set; }
        public DateTime BirthDay { get; set; }

        public string City { get; set; }
        public Gender Gender { get; set; }
    }
}
