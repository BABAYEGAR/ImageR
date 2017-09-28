using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class ImageRating
    {
        public long ImageRatingId { get; set; }
        public long? Rating { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
