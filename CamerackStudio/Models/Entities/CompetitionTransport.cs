using System.Collections.Generic;

namespace CamerackStudio.Models.Entities
{
    public class CompetitionTransport
    {
        public List<AppUser> AppUsers { get; set; }
        public List<PushNotification> SystemNotifications { get; set; }
        public Competition Competition { get; set; }
    }
}
