using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class Subscription : Transport
    {
        public long SubscriptionId { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
