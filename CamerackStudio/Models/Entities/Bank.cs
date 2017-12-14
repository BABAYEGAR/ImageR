using System.Collections.Generic;

namespace CamerackStudio.Models.Entities
{
    public class Bank
    {
        public long BankId { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserBank> UserBanks { get; set; }
    }
}
