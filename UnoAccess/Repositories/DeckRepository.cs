using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class DeckRepository : IDeckRepository
{
    private readonly UnoContext _context;
    private DbSet<Deck> Decks { get; set; }
    
    public DeckRepository(UnoContext context)
    {
        Decks = context.Set<Deck>();
        _context = context;
    }

    public async Task<Deck?> GetDeckByIdAsync(Guid deckId)
    {
        return await Decks.Include(d => d.TopCard).FirstOrDefaultAsync(d => d.Id == deckId);
    }

    public async Task<IEnumerable<Deck>> GetAllDecksAsync()
    {
        return await Decks.ToListAsync();
    }

    public async Task<Deck> CreateDeckAsync(Deck deck)
    {
        using (var db = new UnoContext())
        {
            await db.Decks.AddAsync(deck);

            await db.SaveChangesAsync();
        }
        
        return deck;
    }

    public async Task<Deck> UpdateDeckAsync(Deck deck)
    {
        using (var db = new UnoContext())
        {
            db.Decks.Update(deck);

            await db.SaveChangesAsync();
        }

        return deck;
    }

    public async Task DeleteDeckAsync(Guid deckId)
    {
        var deck = await Decks.FindAsync(deckId);

        if (deck != null)
        {
            Decks.Remove(deck);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Card> GetTopCard(Guid deckId)
    {
        using (var db = new UnoContext())
        {
            var topCardId = db.DiscardPileDecks.Where(d => d.DeckId == deckId).OrderByDescending(c => c.Added).First();

            var topCard = await db.Cards.FindAsync(topCardId);

            return topCard;
        }
    }

    public async Task<Player> GetTurn(Guid deckId)
    {
        using (var db = new UnoContext())
        {
            var deck = await db.Decks.Include(p => p.CurrentPlayer).FirstOrDefaultAsync(d => d.Id == deckId);

            return deck.CurrentPlayer;
        }
    }

}