using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class SliderImage : Transport
    {
        public long SliderImageId { get; set; }
        [Required]
        public string SliderName { get; set; }

        public string File { get; set; }
        [Required]
        public string MainHeaderText { get; set; }
        [Required]
        public string SubHeaderText { get; set; }
        public string Style { get; set; }
    }
}
