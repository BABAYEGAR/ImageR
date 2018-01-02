namespace CamerackStudio.Models.Entities
{
    public class AppUserTransport
    {
        public AppUser AppUser { get; set; }
        public string Role { get; set; }
        public AppUserAccessKey AppUserAccessKey { get; set; }
        public string RequestType { get; set; }
    }
}
