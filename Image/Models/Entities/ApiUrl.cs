using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Models.Entities
{
    public class ApiUrl : Transport
    {
        public long ApiUrlId { get; set; }
        public string RegisterUsersUrl { get; set; }
        public string FetchUsersUrl { get; set; }
        public string ForgotPasswordLinkUrl { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string ChangePasswordrl { get; set; }
        public string EditProfileUrl { get; set; }
        public long TenancyId { get; set; }
    }
}
