using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class Role : Transport
    {
        public long RoleId { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Manage Users")]
        public bool ManageApplicationUser { get; set; }
        [DisplayName("Manage Roles")]
        public bool ManageRoles { get; set; }
        [DisplayName("Manage Clients")]
        public bool ManageClients { get; set; }
        [DisplayName("Manage Access Logs")]
        public bool ManageAccessLogs { get; set; }
        [DisplayName("Upload Images")]
        public bool UploadImage { get; set; }
        [DisplayName("Manage Competitions")]
        public bool ManageCompetition { get; set; }
        [DisplayName("Participate In Competitions")]
        public bool ParticipateCompetition { get; set; }
        [DisplayName("Purchase Images")]
        public bool PurchaseImage { get; set; }
        [DisplayName("Manage Images")]
        public bool ManageImages { get; set; }
        [DisplayName("Manage Image Categories")]
        public bool ManageImageCategory { get; set; }
        [DisplayName("Manage Photographer Categories")]
        public bool ManagePhotographerCategory { get; set; }
        [DisplayName("Manage Cameras")]
        public bool ManageCameras { get; set; }
        [DisplayName("Manage Orders")]
        public bool ManageOrders { get; set; }
        [DisplayName("Manage Payments")]
        public bool ManagePayments { get; set; }
        [DisplayName("Manage Locations")]
        public bool ManageLocations { get; set; }
    }
}
