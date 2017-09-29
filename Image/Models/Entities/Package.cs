using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Image.Models.Entities
{
    public class Package : Transport
    {
        public long PackageId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long? Amount { get; set; }
        public IEnumerable<PackageItem> PackageItems { get; set; }
        public IEnumerable<UserSubscription> UserSubscriptions { get; set; }
    }
}
