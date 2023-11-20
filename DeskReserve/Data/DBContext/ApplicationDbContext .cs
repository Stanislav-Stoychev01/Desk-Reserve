using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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

        public DbSet<Request> Requests { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Request>()
				.HasOne(e => e.Desk)
				.WithMany(e => e.Requests)
				.HasForeignKey(e => e.DeskId);
		}
	}
}