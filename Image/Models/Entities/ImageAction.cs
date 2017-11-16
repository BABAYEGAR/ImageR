using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Models.Entities
{
    public class ImageAction 
    {
        public long ImageActionId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public long? AppUserId { get; set; }
        public long? Rating { get; set; }
        public long? OwnerId { get; set; }
        public long ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public long TenancyId { get; set; }
    }
}
