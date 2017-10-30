using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class CompetitionVote : Transport
    {
        public long CompetitionVoteId { get; set; }
        public long Votes { get; set; }
        //user
        public long? AppUserId { get; set; }
    }
}
