namespace Image.Models.Entities
{
    public class Payment : Transport
    {
        public long PaymentId { get; set; }
        public string Description { get; set; }
        public long? Amount { get; set; }
        public string PaymentMethod { get; set; }
        public long? AppUserId { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
    }
}
