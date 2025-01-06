using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class DiscardPileDeckRepository : IDiscardPileDeckRepository
{
    public async Task<IEnumerable<Guid>> GetDiscardPilesByDeckId(Guid deckId)
    {
        using (var db = new UnoContext())
        {
            var cards = await db.DiscardPileDecks.Where(c => c.DeckId == deckId).ToListAsync();

            return cards.Select(c => c.CardId);
        }
    }

    public Task<Card> AddDiscardPileToDeck(Guid deckId)
    {
        throw new NotImplementedException();
    }

    public Task<Card> GetTopCard()
    {
        throw new NotImplementedException();
    }

    public async Task AddDiscardPile(DiscardPileDeck dp)
    {
        using (var db = new UnoContext())
        {
            await db.DiscardPileDecks.AddAsync(dp);

            await db.SaveChangesAsync();
        }
    }

    public async Task RemoveDiscardPiles(Guid deckId)
    {
        using (var db = new UnoContext())
        {
            var discardPiles = db.DiscardPileDecks.Where(dp => dp.DeckId == deckId);
            db.DiscardPileDecks.RemoveRange(discardPiles);

            await db.SaveChangesAsync();
        }
    }

    Task<IEnumerable<Card>> IDiscardPileDeckRepository.GetDiscardPilesByDeckId(Guid deckId)
    {
        throw new NotImplementedException();
    }
}