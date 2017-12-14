using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamerackStudio.Models.Entities
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
        public long ClientId { get; set; }
    }
}
