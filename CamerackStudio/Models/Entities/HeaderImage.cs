using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class HeaderImage : Transport
    {
        public long HeaderImageId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PageCategory { get; set; }
        public string File { get; set; }
    }
}
