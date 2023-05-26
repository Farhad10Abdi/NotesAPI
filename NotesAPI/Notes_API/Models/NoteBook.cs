using System.ComponentModel.DataAnnotations;

namespace Notes_API.Models
{
    public class NoteBook
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}

        public ICollection<Notes> Notes { get; set; } 

    }
}
