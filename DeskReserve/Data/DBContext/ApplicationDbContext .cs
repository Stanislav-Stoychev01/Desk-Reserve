using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Data.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

		public DbSet<Room> Rooms { get; set; }
        
        public DbSet<Building> Buildings { get; set; }

        public DbSet<Floor> Floor { get; set; }

        public DbSet<Desk> Desks { get; set; }
    }
}