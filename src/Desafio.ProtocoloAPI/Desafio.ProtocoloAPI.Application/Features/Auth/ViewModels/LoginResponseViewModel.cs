namespace Desafio.ProtocoloAPI.Application.Features.Auth.ViewModels;

public class LoginResponseViewModel
{
    public string AccessToken { get; set; } = string.Empty;
    public double ExpiresIn { get; set; }
    public UserTokenViewModel UserToken { get; set; } = new UserTokenViewModel();
}
