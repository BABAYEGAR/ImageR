using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class CompetitionVote : Transport
    {
        public long CompetitionVoteId { get; set; }
        public long? AppUserId { get; set; }
        public long? OwnerId { get; set; }
        public long CompetitionUploadId { get; set; }
        [ForeignKey("CompetitionUploadId")]
        public CompetitionUpload CompetitionUpload { get; set; }
    }
}
