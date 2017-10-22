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
        public long? ImageUploadNumber { get; set; }
        [Display(Name = "Participate In Competitions")]
        public bool Competition { get; set; }
        [Display(Name = "Image Priority")]
        public bool ImagePriority { get; set; }
        [Display(Name = "Contract Certified")]
        public bool Contracts { get; set; }
        public IEnumerable<PackageItem> PackageItems { get; set; }
        public IEnumerable<UserSubscription> UserSubscriptions { get; set; }
    }
}
