using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class ImageCompetitionRating : Transport
    {
        public long ImageCompetitionRatingId { get; set; }
        [Display(Name = "Date Delivery")]
        public long? TimeDeliveryRating { get; set; }
        [Display(Name = "Description")]
        public long? DescriptionRating { get; set; }
        [Display(Name = "Tags")]
        public long? TagsRating { get; set; }
        [Display(Name = "User Acceptance")]
        public long? AcceptanceRating { get; set; }
        public long? TotalRating { get; set; }
        public long? CompetitionId { get; set; }
        public long? AppUserId { get; set; }
    }
}
