using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Notes_API.Models;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Reflection.Metadata.BlobBuilder;


namespace Notes_API.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notes>()
             .HasOne(e => e.NoteBook)
             .WithMany(e => e.Notes)
             .HasForeignKey(e => e.NoteBookId)
             .IsRequired();
        }

    }

    
}
