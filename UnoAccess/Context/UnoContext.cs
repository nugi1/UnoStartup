using Microsoft.EntityFrameworkCore;
using UnoAccess.Entities;
using UnoAccess.PredefinedData;

namespace UnoAccess.Context;

public class UnoContext(DbContextOptions<UnoContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<RoundCard> RoundCards { get; set; }
    public DbSet<PlayerCard> PlayerCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>(e =>
        {
            e.HasKey(g => g.Id);

            e.HasOne(g => g.Round)
                .WithOne(r => r.Game)
                .HasForeignKey<Game>(g => g.RoundId);
        });

        modelBuilder.Entity<Round>(e =>
        {
            e.HasKey(r => r.Id);

            e.HasOne(r => r.Game)
                .WithOne(g => g.Round)
                .HasForeignKey<Round>(r => r.GameId);

            e.HasOne(r => r.TopCard)
                .WithMany()
                .HasForeignKey(r => r.TopCardId);
        });

        modelBuilder.Entity<Player>(e =>
        {
            e.HasKey(p => p.Id);

            e.HasOne(p => p.Game)
                .WithMany(g => g.Players)
                .HasForeignKey(p => p.GameId);
        });

        modelBuilder.Entity<Card>(e =>
        {
            e.HasKey(c => c.Id);

            e.HasData(CardsData.Get);
        });

        modelBuilder.Entity<RoundCard>(e =>
        {
            e.HasKey(c => new { c.RoundId, c.CardId });

            e.HasOne(rc => rc.Round)
                .WithMany(r => r.CardsInPlay)
                .HasForeignKey(rc => rc.RoundId);
        });

        modelBuilder.Entity<PlayerCard>(e =>
        {
            e.HasKey(pc => new { pc.CardId, pc.PlayerId });

            e.HasOne<Card>(pc => pc.Card)
                .WithMany()
                .HasForeignKey(pc => pc.CardId);

            e.HasOne<Player>(pc => pc.Player)
                .WithMany()
                .HasForeignKey(pc => pc.PlayerId);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            @"Host=localhost;Database=GameDatabasee;Username=postgres;Password=Road2Globlob123;TrustServerCertificate=True;");
    }
}