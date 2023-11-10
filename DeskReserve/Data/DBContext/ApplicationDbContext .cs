using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Data.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        
        public DbSet<Building> Buildings { get; set; }

        public DbSet<Floor> Floor { get; set; }

        public DbSet<Desk> Desks { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}