using System.ComponentModel.DataAnnotations;

namespace NeHauar.DTO
{
    public class ChatDto
    {
        [Required] 
        public int FirstUserId { get; set; }
        
        [Required]
        public int SecondUserId { get; set; }
    }
}