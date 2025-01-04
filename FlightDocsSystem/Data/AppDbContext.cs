using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            builder.Entity<Account>()
                .HasOne(a => a.Permission)
                .WithMany(gp => gp.Accounts)
                .HasForeignKey(a => a.PermissionName)
                .HasPrincipalKey(gp => gp.GroupPermissionName);

            builder.Entity<GroupPermission>()
                .HasIndex(gp => gp.GroupPermissionName)
                .IsUnique();
         


        }

    }
}
