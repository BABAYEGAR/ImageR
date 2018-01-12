using CamerackStudio.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CamerackStudio.Models.DataBaseConnections
{
    public class CamerackStudioDataContext : DbContext
    {
        // Your context has been configured to use a 'VmsDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // ' Imager.DataManager.DataBaseConnections.VMSConnections' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ImageDataContext' 
        // connection string in the application configuration file.
        public CamerackStudioDataContext(DbContextOptions<CamerackStudioDataContext> options)
            : base(options)
        {
        }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImageTag> ImageTags { get; set; }
        public virtual DbSet<ImageCompetitionRating> ImageCompetitionRatings { get; set; }
        public virtual DbSet<ImageComment> ImageComments { get; set; }
        public virtual DbSet<ImageCategory> ImageCategories { get; set; }
        public virtual DbSet<ImageAction> ImageActions { get; set; }
        public virtual DbSet<ImageSubCategory> ImageSubCategories { get; set; }
        public virtual DbSet<PhotographerCategory> PhotographerCategories { get; set; }
        public virtual DbSet<Camera> Cameras { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PhotographerCategoryMapping> PhotographerCategoryMappings { get; set; }
        public virtual DbSet<ImageReport> ImageReports { get; set; }
        public virtual DbSet<UserBank> UserBanks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=.;Database=CamerackStudio;Trusted_Connection=True;");
        }
    }
}
