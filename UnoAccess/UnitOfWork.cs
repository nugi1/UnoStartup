using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using UnoAccess.Interfaces;
using UnoAccess.Context;
using System.Data;

namespace UnoAccess;

public class UnitOfWork(
    UnoContext context,
    ICardRepository cardRepository,
    IRoundCardRepository cardRoundRepository,
    IPlayerRepository playerRepository,
    IGameRepository gameRepository,
    IRoundRepository roundRepository,
    IPlayerCardRepository playerCardRepository)
    : IUnitOfWork
{
    private bool _disposed;

    public ICardRepository Cards { get; } = cardRepository;

    public IRoundRepository Rounds { get; } = roundRepository;

    public IRoundCardRepository RoundCards { get; } = cardRoundRepository;

    public IPlayerRepository Players { get; } = playerRepository;

    public IGameRepository Games { get; } = gameRepository;

    public IPlayerCardRepository PlayerCards { get; } = playerCardRepository;

    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return context.Database.BeginTransactionAsync(isolationLevel);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        _disposed = true;
    }
}
