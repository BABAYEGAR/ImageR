using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Image.Models.Entities
{
    public class PhotographerCategory : Transport
    {
        public long PhotographerCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
    }
}
