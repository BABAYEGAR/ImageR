using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class Order : Transport
    {
        public long OrderId { get; set; }
        public string OrderNumber { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public long? AppUserId { get; set; }
    }
}
