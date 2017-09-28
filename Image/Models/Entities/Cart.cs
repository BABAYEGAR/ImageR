using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class Cart : Transport
    {
        public long CartId { get; set; }
        public long? Quantity { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
