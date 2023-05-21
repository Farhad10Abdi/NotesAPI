using System.Linq.Expressions;

namespace Notes_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task CreateAsync(T entity);
        Task RemoveAsync(int id);
        Task SaveChangesAsync();
    }
}
