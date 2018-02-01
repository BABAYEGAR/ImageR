using System;
using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class Advertisement : Transport
    {
        public long AdvertisementId { get; set; }
        [Required]
        [Display(Name = "Ad Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Client Name")]
        public string Client { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string ClientEmail { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "This field must contain only digits")]
        [Display(Name = "Mobile")]
        public string ClientPhoneNumber { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Website { get; set; }
        public string File { get; set; }
        [Display(Name = "Ad Click")]
        public long? AdClick { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string PageCategory { get; set; }
    }
}
