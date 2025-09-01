using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StajProjesi.Models;

namespace StajProjesi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<Hospital> Hospitals => Set<Hospital>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // İstersen burada Hospital için ek kurallar/indeksler verebilirsin.
        // builder.Entity<Hospital>().HasIndex(h => h.Name);
    }

    public DbSet<TransferRequest> TransferRequests { get; set; } = default!;

}