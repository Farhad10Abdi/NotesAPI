using Notes_API.Data;
using Notes_API.Models;
using Notes_API.Repository.IRepository;

namespace Notes_API.Repository
{
    public class NoteRepository : Repository<Notes>, INoteRepository
    {
        private readonly ApplicationDbContext _db;
        public NoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Notes> UpdateAsync(Notes notes)
        {
            notes.UpdatedDate = DateTime.Now;
            _db.Notes.Update(notes);
            await _db.SaveChangesAsync();

            return notes;
        }
    }
}
