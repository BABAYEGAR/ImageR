using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class UserSubscription : Transport
    {
        public long UserSubscriptionId { get; set; }
        public string Status { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public long? PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }

    }
}
