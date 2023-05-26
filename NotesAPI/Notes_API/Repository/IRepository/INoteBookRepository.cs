using Notes_API.Models;

namespace Notes_API.Repository.IRepository
{
    public interface INoteBookRepository : IRepository<NoteBook>
    {
        Task<NoteBook> UpdateAsync(NoteBook noteBook);
    }
}
