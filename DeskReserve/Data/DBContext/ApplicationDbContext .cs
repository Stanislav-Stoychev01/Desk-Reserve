using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Data.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

        public DbSet<Floor> Floors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DeskReserve;Trusted_Connection=True;");
        }

    }
}