namespace Notes_API.Models
{
    public class NoteBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}

        public ICollection<Notes> Notes { get; set; } 

    }
}
