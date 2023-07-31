using AuthApi.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Models.Application
{
    public class Application
    {
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Enabled { get; set; }
        public string AppGuid { get; set; }
        public List<User> Users { get; set; }
        //public List<UserApplication> UserApplications { get; set; }

    }
}
