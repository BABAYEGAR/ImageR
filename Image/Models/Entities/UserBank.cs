using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Image.Models.Entities
{
    public class UserBank : Transport
    {
        public long UserBankId { get; set; }
        [Display(Name = "Account Number")]
        public long? AccountNumber { get; set; }
        [Display(Name = "Account Name")]
        public  string AccountName { get; set; }
        [Display(Name = "Tax Identification No")]
        public string Tin { get; set; }
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
        [Display(Name = "Bank")]
        public long? BankId { get; set; }
        [ForeignKey("BankId")]
        public Bank Bank { get; set; }
    }
}
