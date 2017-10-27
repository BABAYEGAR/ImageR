using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class Image : Transport
    {
        public long ImageId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Theme { get; set; }
        public string Caption { get; set; }
        [Required]
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [Display(Name = "Camera")]
        public long? CameraId { get; set; }
        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }
        [Display(Name = "Location")]
        public long? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
        [Display(Name = "Selling Price")]
        [Required]
        public long? SellingPrice { get; set; }
        public string Inspiration { get; set; }
        [Display(Name = "Sub-Category/Sub-Genre")]
        public long? ImageSubCategoryId { get; set; }
        [ForeignKey("ImageSubCategoryId")]
        public ImageSubCategory ImageSubCategory { get; set; }
        [Required]
        [Display(Name = "Category/Genre")]
        public long? ImageCategoryId { get; set; }
        [ForeignKey("ImageCategoryId")]
        public ImageCategory ImageCategory { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public long? Like { get; set; }
        public long? DisLike { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
        public IEnumerable<ImageTag> ImageTags { get; set; }
        public IEnumerable<ImageComment> ImageComments { get; set; }
    }
}
