namespace CamerackStudio.Models.Entities
{
    public class SearchCriteria
    {
        public long? MinimumPrice { get; set; }
        public long? MaximumPrice { get; set; }
        public long? Rating { get; set; }
        public long? ImageCategoryId { get; set; }
        public long? PhotographerCategoryId { get; set; }
        public long? LocationId { get; set; }
        public long? CameraId { get; set; }
        public long? ImageSubCategoryId { get; set; }
    }
}
