using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Models.Configurations;

namespace Revenue_Recognition_System.Context;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppUserEfConfiguration).Assembly);
    }


    public DbSet<AppUser> AppUsers { get; init; }
    public DbSet<Individual> Individuals { get; init; }
    public DbSet<Company> Companies { get; init; }
    public DbSet<Discount> Discounts { get; init; }
    public DbSet<Software> Softwares { get; init; }
    public DbSet<SoftwareContract> SoftwareContracts { get; init; }
    public DbSet<Payment> Payments { get; set; }
}