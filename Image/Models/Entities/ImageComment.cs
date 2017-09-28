using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class ImageComment
    {
        public long ImageCommentId { get; set; }
        public string Comment { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
