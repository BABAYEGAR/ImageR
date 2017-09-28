using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class AppUser : Transport
    {
        public long AppUserId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("[a-zA-Z ]*$")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("^[0-9]*$")]
        public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public long? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public string Status { get; set; }

        [DisplayName("Profile Picture")]
        public string ProfilePicture { get; set; }

        public string DisplayName
            => Firstname + " " + Lastname;

        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<BillingAddress>BillingAddresses { get; set; }
        public IEnumerable<ShippingAddress> ShippingAddresses { get; set; }
        public IEnumerable<UserSubscription> UserSubscriptions { get; set; }
        public IEnumerable<Competition> Competitions { get; set; }
        public IEnumerable<CompetitionVote> CompetitionVotes { get; set; }
    }
}
