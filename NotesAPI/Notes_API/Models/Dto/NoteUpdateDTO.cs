using System.ComponentModel.DataAnnotations;

namespace Notes_API.Models.Dto
{
    public class NoteUpdateDTO
    {
        [Required]
        public int ID { get; set; }
        public string Text { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
