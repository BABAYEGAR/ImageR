using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Image.Models.Entities
{
    public class UserSubscription : Transport
    {
        public long UserSubscriptionId { get; set; }
        public string Status { get; set; }
        public long? AppUserId { get; set; }
        public long? PackageId { get; set; }
        [ForeignKey("PackageId")]
        [JsonIgnore]
        public Package Package { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long? MonthLength { get; set; }
        public long? Amount { get; set; }

    }
}
