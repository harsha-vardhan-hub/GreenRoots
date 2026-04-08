using Microsoft.EntityFrameworkCore;
using GreenRoots.API.Models;

namespace GreenRoots.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TreeRequest> TreeRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Ensure email is unique (conceptually)
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Relationship (One User -> Many Requests)
        modelBuilder.Entity<TreeRequest>()
            .HasOne(t => t.User)
            .WithMany() // Assuming we don't need navigation prop inside User for requests
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
