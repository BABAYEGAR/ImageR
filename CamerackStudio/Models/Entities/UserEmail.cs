using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamerackStudio.Models.Entities
{
    public class UserEmail
    {
        public List<AppUser> AppUsers { get; set; }
        public string Body { get; set; }
        public string MessageCategory { get; set; }
    }
}
