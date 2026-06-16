namespace RecrutamentoDiversidade.Repositories.Interfaces;

using RecrutamentoDiversidade.Models;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<bool> ExistePorEmailAsync(string email);
    Task<Usuario> CriarAsync(Usuario usuario);
}