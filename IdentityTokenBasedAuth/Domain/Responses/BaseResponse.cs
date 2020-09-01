using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Domain.Responses
{
    public class BaseResponse<T> where T:class
    {
        public T Extra { get; set; }
        // kullanıciya veri dönerken gercekten bu id ye ait bir kullanici varsa Successle birlikte kullanicinin istedigi bilgiyi donecegiz 
        public bool Success { get; set; }
        public string Message { get; set; }


        public BaseResponse(T extra=null)
        {
            this.Extra = extra;
            this.Success = true;
        }
        public BaseResponse(string message)
        {
            this.Success = false;
            this.Message = message;
        }
    }
}
