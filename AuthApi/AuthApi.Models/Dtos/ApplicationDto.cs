using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Models.Dtos
{
    public class ApplicationDto
    {
        public string ApplicationName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Enabled { get; set; }
        public string AppGuid { get; set; } = new Guid().ToString();
    }
}
