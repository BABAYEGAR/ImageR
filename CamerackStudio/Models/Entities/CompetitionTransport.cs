using System.Collections.Generic;

namespace CamerackStudio.Models.Entities
{
    public class CompetitionTransport
    {
        public List<AppUser> AppUsers { get; set; }
        public List<SystemNotification> SystemNotifications { get; set; }
        public Competition Competition { get; set; }
    }
}
