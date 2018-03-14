using System.Collections.Generic;

namespace CamerackStudio.Models.Entities
{
    public class AppTransport
    {
        public List<Image> Images { get; set; }
        public List<Camera> Cameras { get; set; }
        public List<Location> Locations { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }
        public List<PhotographerCategory> PhotographerCategories { get; set; }
        public List<PhotographerCategoryMapping> PhotographerCategoryMappings { get; set; }
        public List<ImageCategory> Categories { get; set; }
        public List<ImageComment> ImageComments { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<ImageAction> ImageActions { get; set; }
        public List<ImageDownload> ImageDownloads { get; set; }
        public Image Image { get; set; }
        public AppUser AppUser { get; set; }
        public string Status { get; set; }
        public Role Role { get; set; }
    }
}
