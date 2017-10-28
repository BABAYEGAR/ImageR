using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class ImageCompetitionRating : Transport
    {
        public long ImageCompetitionRatingId { get; set; }
        [Display(Name = "Concept")]
        public long ConceptRating { get; set; }
        [Display(Name = "Clearity")]
        public long ClearityRating { get; set; }
        [Display(Name = "Quality")]
        public long QualityRating { get; set; }
        [Display(Name = "Date Delivery")]
        public long TimeDeliveryRating { get; set; }
        [Display(Name = "Description")]
        public long DescriptionRating { get; set; }
        [Display(Name = "User Acceptance")]
        public long AcceptanceRating { get; set; }
        public long TotalRating { get; set; }
        public long CompetitionUploadId { get; set; }
        [ForeignKey("CompetitionUploadId")]
        public CompetitionUpload CompetitionUpload { get; set; }
    }
}
