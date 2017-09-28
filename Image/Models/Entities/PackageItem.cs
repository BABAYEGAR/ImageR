using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class PackageItem
    {
        public long PackageItemId { get; set; }
        [Required]
        public string Name { get;set; }
        public long? PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }
    }
}
