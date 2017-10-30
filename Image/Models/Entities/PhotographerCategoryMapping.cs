using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Models.Entities
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
