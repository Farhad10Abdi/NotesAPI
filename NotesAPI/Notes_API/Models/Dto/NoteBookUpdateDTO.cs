using System.ComponentModel.DataAnnotations;

namespace Notes_API.Models.Dto
{
    public class NoteBookUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
