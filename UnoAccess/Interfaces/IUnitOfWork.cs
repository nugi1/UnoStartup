using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace UnoAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICardRepository Cards { get; }

    IRoundCardRepository RoundCards { get; }

    IRoundRepository Rounds { get; }

    IPlayerRepository Players { get; }

    IGameRepository Games { get; }
    
    IPlayerCardRepository PlayerCards { get; }

    Task<int> SaveAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}
