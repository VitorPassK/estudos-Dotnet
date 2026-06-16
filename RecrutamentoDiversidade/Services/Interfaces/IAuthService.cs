namespace RecrutamentoDiversidade.Services.Interfaces;

using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegistroAsync(RegistroRequest request);
}