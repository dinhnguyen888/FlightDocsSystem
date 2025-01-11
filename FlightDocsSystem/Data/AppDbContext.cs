using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FlightDocsSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<DocsType> DocsTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            builder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleName)
                .HasPrincipalKey(gp => gp.RoleName);

            builder.Entity<Role>()
                .HasIndex(gp => gp.RoleName)
                .IsUnique();

            builder.Entity<Permission>()
                .HasKey(p => new { p.DocsTypeId, p.RoleId });

            builder.Entity<Permission>()
                .HasOne(p => p.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(p => p.RoleId);

            builder.Entity<Permission>()
                .HasOne(p => p.DocsType)
                .WithMany(r => r.Permissions)
                .HasForeignKey(p => p.DocsTypeId);

            builder.Entity<Document>()
                .HasOne(d => d.DocsType)
                .WithMany(r => r.Documents)
                .HasForeignKey(d => d.DocsTypeId);

            builder.Entity<Document>()
                .HasOne(d => d.Flight)
                .WithMany(r => r.Documents)
                .HasForeignKey(d => d.FlightId);


        }

    }
}
