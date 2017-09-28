using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class UserSubscription : Transport
    {
        public long UserSubscriptionId { get; set; }
        public long? NumberOfImagesDownloaded { get; set; }
        public long Status { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public long? PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }
        public long? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
