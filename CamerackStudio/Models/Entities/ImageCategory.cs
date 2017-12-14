using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class ImageCategory : Transport
    {
        public long ImageCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public string FileName { get; set; }
        public IEnumerable<ImageSubCategory> ImageSubCategories { get; set; }
    }
}
