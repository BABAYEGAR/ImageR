using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Models.Entities
{
    public class CompetitionCategory : Transport
    {
        public long CompetitionCategoryId { get; set; }
        public long CompetitionId { get;set; }
        [ForeignKey("CompetitionId")]
        public Competition Competition { get; set; }
        public long PhotographerCategoryId { get; set; }
        [ForeignKey("PhotographerCategoryId")]
        public PhotographerCategory PhotographerCategory { get; set; }
    }
}
