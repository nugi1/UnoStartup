using Microsoft.EntityFrameworkCore;
using UnoAccess.Entities;
using UnoAccess.PredefinedData;

namespace UnoAccess.Context;

public class UnoContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<CardDeck> CardDecks { get; set; }
    public DbSet<DiscardPileDeck> DiscardPileDecks { get; set; }

    public DbSet<CardPlayer> CardPlayers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.HasOne<Player>(g => g.Winner).WithMany().HasForeignKey(g => g.WinnerId);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne<Deck>(e => e.Deck).WithMany(e => e.Players).HasForeignKey(e => e.DeckId);
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasData(CardsData.GetData);
        });

        modelBuilder.Entity<Deck>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasOne(c => c.CurrentPlayer).WithMany().HasForeignKey(e => e.CurrentPlayerId);
        });

        modelBuilder.Entity<CardDeck>(entity =>
        {
            entity.HasKey(e => new { e.CardId, e.DeckId });
        });

        modelBuilder.Entity<CardPlayer>(entity =>
        {
            entity.HasKey(e => new { e.CardId, e.PlayerId });
        });

        modelBuilder.Entity<DiscardPileDeck>(entity =>
        {
            entity.HasKey(e => new { e.CardId, e.DeckId });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            @"Host=postgres;Database=GameDatabase;Username=postgres;Password=Road2Globlob123;TrustServerCertificate=True;");
    }
}