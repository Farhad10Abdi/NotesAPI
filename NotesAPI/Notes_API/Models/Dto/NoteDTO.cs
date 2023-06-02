using System.ComponentModel.DataAnnotations;

namespace Notes_API.Models.Dto
{
    public class NoteDTO
    {
        public int ID { get; set; }
        public int NoteBookId { get; set; }
        public string Text { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        
    }
}
