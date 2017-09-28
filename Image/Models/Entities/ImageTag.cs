using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class ImageTag : Transport
    {
        public long ImageTagId { get; set; }
        [Required]
        public string Name { get; set; }
        public long ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }

    }
}
