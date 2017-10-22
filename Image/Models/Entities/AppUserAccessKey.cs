using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Models.Entities
{
    public class AppUserAccessKey : Transport
    {
        public long AppUserAccessKeyId { get; set; }
        public long AppUserId { get;set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public string PasswordAccessCode { get; set; }
        public string AccountActivationAccessCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
