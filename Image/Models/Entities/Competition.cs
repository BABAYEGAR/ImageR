using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Image.Models.Entities
{
    public class Competition : Transport
    {
        public long CompetitionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Theme { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date & Time")]
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        //winner
        public long? AppUserId { get; set; }
        public IEnumerable<CompetitionCategory> CompetitionCategories { get; set; }
        public IEnumerable<CompetitionVote> CompetitionVotes { get; set; }
        public IEnumerable<CompetitionUpload> CompetitionUploads { get; set; }
    }
}
