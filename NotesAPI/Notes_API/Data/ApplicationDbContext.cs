using Microsoft.EntityFrameworkCore;
using Notes_API.Models;

namespace Notes_API.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Notes> Notes { get; set; }
    }
}
