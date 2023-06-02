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

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, int pageSize = 3, int pageNumber = 1)
        {
            IQueryable<T> query = set;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0) 
            {
                query = query.Skip(pageSize * (pageNumber -1)).Take(pageSize);
            }
            return query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = set;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            set.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
