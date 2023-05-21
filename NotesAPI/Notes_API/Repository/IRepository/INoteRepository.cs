using Notes_API.Models;

namespace Notes_API.Repository.IRepository
{
    public interface INoteRepository : IRepository<Notes>
    {
        Task<Notes> UpdateAsync(Notes notes);
    }
}
