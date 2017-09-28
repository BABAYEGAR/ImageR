using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class Image : Transport
    {
        public long ImageId { get; set; }
        public long Title { get; set; }
        public long Description { get; set; }
        public long PerspectOne { get; set; }
        public long PerspectTwo { get; set; }
        public long PerspectThree { get; set; }
        public long? ImageCategoryId { get; set; }
        [ForeignKey("ImageCategoryId")]
        public ImageCategory ImageCategory { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public IEnumerable<ImageClick> ImageClicks { get; set; }
        public IEnumerable<ImageRating> ImageRatings { get; set; }
        public IEnumerable<ImageTag> ImageTags { get; set; }
        public IEnumerable<ImageComment> ImageComments { get; set; }
    }
}
