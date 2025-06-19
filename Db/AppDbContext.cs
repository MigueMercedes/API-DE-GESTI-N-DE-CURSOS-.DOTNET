using Microsoft.EntityFrameworkCore;
using study.models;

namespace study.db
{
  public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
  {
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>()
          .HasOne(u => u.Role)
          .WithMany()
          .HasForeignKey(u => u.RoleId)
          .IsRequired();
    }
  }
}