using System.ComponentModel.DataAnnotations;

namespace Notes_API.Models.Dto
{
    public class NoteBookCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        
    }
}
