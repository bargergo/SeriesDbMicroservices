using Microsoft.EntityFrameworkCore;
using UserServer.Entities;
using UserServer.Models;

namespace UserServer.Database
{
    public class UserDbContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<ApplicationUserRole> UserRoles { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

                b.HasIndex(e => e.Email)
                    .IsUnique();

                b.HasIndex(e => e.UserName)
                    .IsUnique();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasIndex(e => e.NormalizedName)
                    .IsUnique();
            });

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = 1,
                Name = RoleNames.AdministratorRoleName,
                NormalizedName = RoleNames.AdministratorRoleName.ToUpper()
            });

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = 2,
                Name = RoleNames.UserRoleName,
                NormalizedName = RoleNames.UserRoleName.ToUpper()
            });
        }
    }
}
