using AuthApi.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Models.Application
{
    public class UserApplication
    {
        public string UserId { get; set; }
        public int ApplicationId { get; set; }
        //public User Users { get; set; }
        //public Application Applications { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool Enable { get; set; }
    }
}
