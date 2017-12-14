using System.ComponentModel.DataAnnotations.Schema;

namespace CamerackStudio.Models.Entities
{
    public class PhotographerCategoryMapping : Transport
    {
        public long PhotographerCategoryMappingId { get; set; }
        public long AppUserId { get;set; }
        public long PhotographerCategoryId { get; set; }
        [ForeignKey("PhotographerCategoryId")]
        public PhotographerCategory PhotographerCategory { get; set; }
    }
}
