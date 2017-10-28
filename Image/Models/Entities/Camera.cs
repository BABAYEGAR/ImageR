using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Image.Models.Entities
{
    public class Camera : Transport
    {
        public long CameraId { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<CompetitionUpload> CompetitionUploads { get; set; }
    }
}
