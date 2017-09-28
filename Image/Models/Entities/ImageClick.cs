using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class ImageClick : Transport
    {
        public long ImageClickId { get; set; }
        public long Like { get; set; }
        public long DisLike { get; set; }
        public string Action { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
