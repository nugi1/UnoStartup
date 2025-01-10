using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using UnoAccess.Interfaces;
using UnoAccess.Context;
using System.Data;

namespace UnoAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly UnoContext _context;

    private bool _disposed;

    public UnitOfWork(
        UnoContext context,
        ICardRepository cardRepository,
        IRoundCardRepository cardRoundRepository,
        IPlayerRepository playerRepository,
        IGameRepository gameRepository,
        IRoundRepository roundRepository)
    {
        _context = context;
        Cards = cardRepository;
        Rounds = roundRepository;
        RoundCards = cardRoundRepository;
        Games = gameRepository;
        Players = playerRepository;
    }

    public ICardRepository Cards { get; private set; }

    public IRoundRepository Rounds { get; private set; }

    public IRoundCardRepository RoundCards { get; private set; }

    public IPlayerRepository Players { get; private set; }

    public IGameRepository Games { get; private set; }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public Task<int> SaveAsync(bool logChanges)
    {
        return SaveAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return _context.Database.BeginTransactionAsync(isolationLevel);
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
                _context.Dispose();
            }
        }

        _disposed = true;
    }
}
