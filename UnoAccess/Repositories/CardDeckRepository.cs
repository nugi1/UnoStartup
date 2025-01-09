using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class CardDeckRepository : ICardDeckRepository
{
    public async Task<IEnumerable<CardDeck>> GetCardsByDeck(Guid deckId)
    {
        using (var db = new UnoContext())
        {
            var cards = await db.CardDecks.Where(d => d.DeckId == deckId).ToListAsync();

            return cards;
        }
    }
    public async Task FillTheDeck(IList<CardDeck> cards, Guid deckId)
    {
        Shuffle<CardDeck>(cards);
        var cardDecks = cards.Select((c, index) => new CardDeck()
        {
            CardId = c.CardId,
            DeckId = deckId,
            Position = index
        });


        using (var db = new UnoContext())
        {
            foreach (var cardDeck in cardDecks)
            {
                await db.CardDecks.AddAsync(cardDeck);
            }

            await db.SaveChangesAsync();
        }
    }

    public async Task RemoveCardAsync(CardDeck cardDeck)
    {
        using (var db = new UnoContext())
        {
            db.CardDecks.Remove(cardDeck);

            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateRange(IEnumerable<CardDeck> cardDecks)
    {
        using (var db = new UnoContext())
        {
            foreach (var cardDeck in cardDecks)
            {
                db.CardDecks.Update(cardDeck);
            }

            await db.SaveChangesAsync();
        }
    }
    
    public static void Shuffle<T>(IList<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}