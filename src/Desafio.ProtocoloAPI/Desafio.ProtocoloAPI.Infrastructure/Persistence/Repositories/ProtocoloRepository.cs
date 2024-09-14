using Desafio.ProtocoloAPI.Core.Entities;
using Desafio.ProtocoloAPI.Core.Repositories;

namespace Desafio.ProtocoloAPI.Infrastructure.Persistence.Repositories;


public class ProtocoloRepository : Repository<Protocolo>, IProtocoloRepository
{
    public ProtocoloRepository(DataIdentityDbContext db) : base(db)
    {
    }

    
}
