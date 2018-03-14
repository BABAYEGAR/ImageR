using System;

namespace CamerackStudio.Models.Entities
{
    public class ImageDownload : Transport
    {
        public long ImageDownloadId { get; set; }
        public long? ImageId { get; set; }
        public DateTime Date { get; set; }
        public long? AppUserId { get; set; }
        public string Size { get; set; }
    }
}
