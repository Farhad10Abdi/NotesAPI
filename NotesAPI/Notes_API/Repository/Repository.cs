using Microsoft.EntityFrameworkCore;
using Notes_API.Data;
using Notes_API.Repository.IRepository;
using System.Linq.Expressions;

namespace Notes_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> set;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.set = db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await set.AddAsync(entity);
            await SaveChangesAsync();
        }

        public Task<T> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(T entity)
        {
            set.Remove()
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
