using System.ComponentModel.DataAnnotations.Schema;

namespace CamerackStudio.Models.Entities
{
    public class ImageReport : Transport
    {
        public long ImageReportId { get; set; }
        public string Reason { get; set; }
        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
