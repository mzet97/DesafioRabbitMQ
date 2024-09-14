using Desafio.ProtocoloAPI.Core.Events;

namespace Desafio.ProtocoloAPI.Core.Entities;

public class AggregateRoot : IEntityBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    private List<IDomainEvent> _events = new List<IDomainEvent>();
    public IEnumerable<IDomainEvent> Events => _events;

    protected void AddEvent(IDomainEvent @event)
    {
        if (_events == null)
            _events = new List<IDomainEvent>();

        _events.Add(@event);
    }
}
