using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using UnoAccess.Context;
using UnoAccess.Entities;
using UnoAccess.Interfaces;

namespace UnoAccess.Repositories;

public class CardRepository : ICardRepository
{
    private DbSet<Card> Cards { get; set; }
    
    public CardRepository(UnoContext context)
    {
        Cards = context.Set<Card>();
    }

    public async Task<Card?> GetCardByColorAndValue(CardColor color, CardValue value)
    {
        return await Cards.FirstOrDefaultAsync(c => c.Color == color && c.Value == value);
    }

    public async Task<IEnumerable<Card>> GetAllCardsAsync()
    {
        return await Cards.ToListAsync();
    }
}