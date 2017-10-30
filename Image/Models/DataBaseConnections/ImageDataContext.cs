using Image.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Image.Models.DataBaseConnections
{
    public class ImageDataContext : DbContext
    {
        // Your context has been configured to use a 'VmsDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // ' Imager.DataManager.DataBaseConnections.VMSConnections' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ImageDataContext' 
        // connection string in the application configuration file.
        public ImageDataContext(DbContextOptions<ImageDataContext> options)
            : base(options)
        {

        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Entities.Image> Images { get; set; }
        public virtual DbSet<ImageTag> ImageTags { get; set; }
        public virtual DbSet<ImageCompetitionRating> ImageCompetitionRatings { get; set; }
        public virtual DbSet<ImageComment> ImageComments { get; set; }
        public virtual DbSet<ImageCategory> ImageCategories { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<PackageItem> PackageItem { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<ImageSubCategory> ImageSubCategories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }
        public virtual DbSet<Competition> Competition { get; set; }
        public virtual DbSet<PhotographerCategory> PhotographerCategories { get; set; }
        public virtual DbSet<Camera> Cameras { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<CompetitionCategory> CompetitionCategories { get; set; }
        public virtual DbSet<PhotographerCategoryMapping> PhotographerCategoryMappings { get; set; }
        public virtual DbSet<CompetitionUpload> CompetitionUploads { get; set; }

        public virtual DbSet<AppUserAccessKey> AccessKeys { get; set; }
        public virtual DbSet<SystemNotification> SystemNotifications { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<ApiUrl> ApiUrls { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=Image;Trusted_Connection=True;");
        }
    }
}
