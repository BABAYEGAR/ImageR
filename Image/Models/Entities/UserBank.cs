using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Image.Models.Entities
{
    public class UserBank : Transport
    {
        public long UserBankId { get; set; }
        public long? AccountNumber { get; set; }
        public  string AccountName { get; set; }
        public string Tin { get; set; }
        public string AccountType { get; set; }
        public long? BankId { get; set; }
        [ForeignKey("BankId")]
        public Bank Bank { get; set; }
    }
}
