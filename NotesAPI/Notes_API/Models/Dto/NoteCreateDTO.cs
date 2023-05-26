using System.ComponentModel.DataAnnotations;

namespace Notes_API.Models.Dto
{
    public class NoteCreateDTO
    {
        [Required]
        public int NoteBookId { get; set; }

        public string Text { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
