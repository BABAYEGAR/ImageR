using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamerackStudio.Models.Entities
{
    public class PrivacyPolicy : Transport
    {
        public long PrivacyPolicyId { get; set; }
        public string Text { get; set; }
    }
}
