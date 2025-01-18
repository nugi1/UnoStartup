using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoAccess.Entities;

namespace UnoAccess.Interfaces;

public interface IRoundCardRepository : IGenericRepository<RoundCard>
{
    Task DeleteBy(Guid roundId, Guid cardId);

    RoundCard? GetTrackedEntity(object roundId, object cardId);
}
