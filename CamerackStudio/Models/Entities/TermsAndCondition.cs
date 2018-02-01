using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Entities
{
    public class TermAndCondition : Transport
    {
        public long TermAndConditionId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
