using AuthApi.Models.Application;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Models.Authentication
{
    public class User : IdentityUser
    {
        //public List<UserApplication> UserApplications { get; set; }
        public List<Application.Application> Applications { get; set; }
        public bool Approved { get; set; }
    }
}
