namespace Desafio.ProtocoloAPI.Core.Entities
{
    public interface IEntityBase
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
        DateTime? DeletedAt { get; }
    }
}
