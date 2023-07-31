using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Models.Authentication
{
    public class RegisterAdminModel : RegisterModel
    {
        public string SuperSecretKey { get; set; }
    }
}
