using Microsoft.EntityFrameworkCore.Query.Internal;
using Notes_API.Controllers;
using Notes_API.Data;
using Notes_API.Models;
using Notes_API.Repository.IRepository;

namespace Notes_API.Repository
{
    public class NoteBookRepository : Repository<NoteBook>, INoteBookRepository
    {
        private readonly ApplicationDbContext _db;
        public NoteBookRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<NoteBook> UpdateAsync(NoteBook noteBook)
        {
            noteBook.UpdatedDate = DateTime.Now;
            _db.NoteBooks.Update(noteBook);
            await _db.SaveChangesAsync();

            return noteBook;
        }
    }
}
