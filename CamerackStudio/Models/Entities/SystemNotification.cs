namespace CamerackStudio.Models.Entities
{
    public class SystemNotification : Transport
    {
        public long SystemNotificationId { get; set; }
        public string Message { get; set; }
        public long? ControllerId { get; set; }
        public long? AppUserId { get; set; }
        public bool? Read { get; set; }
        public string Category { get; set; }
    }
}
