using System.ComponentModel.DataAnnotations;

namespace eBot.Models
{
    public class User
    {
        [Required]
        public long ChatId { get; set; }
        
        public int WordsLearned { get; set; }
        
        public int WordsInProgress { get; set; }
        
        
    }
}