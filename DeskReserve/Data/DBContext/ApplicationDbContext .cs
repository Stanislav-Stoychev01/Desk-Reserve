using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Data.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<Building> Buildings { get; set; }
    }
}