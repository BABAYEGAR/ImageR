using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class ImageCompetitionRating
    {
        public long ImageCompetitionRatingId { get; set; }
        public long? ConceptRating { get; set; }
        public long? ClearityRating { get; set; }
        public long? QualityRating { get; set; }
        public long? TimeDeliveryRating { get; set; }
        public long? DescriptionRating { get; set; }
        public long CompetitionUploadId { get; set; }
        [ForeignKey("CompetitionUploadId")]
        public CompetitionUpload CompetitionUpload { get; set; }
    }
}
