using Microsoft.EntityFrameworkCore;

namespace PartyPlay.Pg.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Party> Parties { get; set; }
    public DbSet<PartyUser> PartyUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Party>()
            .HasMany(p => p.Users)
            .WithOne(u => u.Party)
            .HasForeignKey(u => u.PartyId);
        
    }
}