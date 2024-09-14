namespace Desafio.ProtocoloAPI.Application.Dtos.InputModels;

public abstract class BaseSearch
{
    public string? Order { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    protected BaseSearch()
    {
        PageIndex = 1;
        PageSize = 10;
    }
}