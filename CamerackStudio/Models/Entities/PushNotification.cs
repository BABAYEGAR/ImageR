namespace CamerackStudio.Models.Entities
{
    public class PushNotification : Transport
    {
        public long PushNotificationId { get; set; }
        public string Message { get; set; }
        public long? ControllerId { get; set; }
        public long? AppUserId { get; set; }
        public bool? Read { get; set; }
        public string Category { get; set; }
        public long? ClientId { get; set; }
    }
}
