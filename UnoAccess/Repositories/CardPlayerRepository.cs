using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoAccess.PredefinedData;

namespace UnoAccess.Repositories;

public class CardPlayerRepository : ICardPlayerRepository
{
    public async Task GiveCardsToPlayers(Guid deckId, List<Guid> cards, List<Guid> playerIds)
    {
        using (var db = new UnoContext())
        {
            for (int i = 0; i < playerIds.Count * 7; i++)
            {
                var cardPlayer = new CardPlayer
                {
                    PlayerId = playerIds[i % playerIds.Count],
                    CardId = cards[i],
                };

                await db.CardPlayers.AddAsync(cardPlayer);
                db.CardDecks.Remove(new CardDeck()
                {
                    DeckId = deckId,
                    CardId = cards[i]
                });
            }
            Console.WriteLine("Aqac modis");
            await db.SaveChangesAsync();
        }
    }

    public async Task GiveCardToPlayer(Guid deckId, Guid playerId, List<Guid> cardIds)
    {
        var cardPlayers = cardIds.Select(c => new CardPlayer()
        {
            CardId = c,
            PlayerId = playerId
        });

        using (var db = new UnoContext())
        {
            foreach (var cardPlayer in cardPlayers)
            {
                await db.CardPlayers.AddAsync(cardPlayer);
                db.CardDecks.Remove(new CardDeck()
                {
                    DeckId = deckId,
                    CardId = cardPlayer.CardId
                });
            }

            await db.SaveChangesAsync();
        }
    }

    public async Task<CardPlayer?> GetCardPlayer(Guid playerId, Guid cardId)
    {
        using (var db = new UnoContext())
        {
            return await db.CardPlayers.FindAsync(cardId, playerId);
        }
    }

    public async Task<IEnumerable<CardPlayer>> GetPlayerCards(Guid playerId)
    {
        using (var db = new UnoContext())
        {
            return await db.CardPlayers.Where(cp => cp.PlayerId == playerId).ToListAsync();
        }
    }

    public async Task RemoveAsync(CardPlayer cardPlayer)
    {
        using (var db = new UnoContext())
        {
            db.CardPlayers.Remove(cardPlayer);

            await db.SaveChangesAsync();
        }
    }
}