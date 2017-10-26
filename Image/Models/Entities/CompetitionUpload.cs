using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Image.Models.Entities
{
    public class CompetitionUpload : Transport
    {
        public long CompetitionUploadId { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public long CompetitionId { get; set; }
        [ForeignKey("CompetitionId")]
        public Competition Competition { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Inspiration { get; set; }
        public string Description { get; set; }
        [Display(Name = "Camera")]
        public long? CameraId { get; set; }
        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }
        [Display(Name = "Location")]
        public long? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
        public long? Like { get; set; }
        public long? DisLike { get; set; }
        public IEnumerable<ImageCompetitionRating> ImageCompetitionRatings { get; set; }

    }
}
