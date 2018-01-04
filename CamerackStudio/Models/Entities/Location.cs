using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class Location : Transport
    {
        public long LocationId { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}
