namespace Desafio.ProtocoloAPI.Application.Dtos.InputModels;

public abstract class BaseSearch
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? Order { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    protected BaseSearch()
    {
        PageIndex = 1;
        PageSize = 10;
    }
}