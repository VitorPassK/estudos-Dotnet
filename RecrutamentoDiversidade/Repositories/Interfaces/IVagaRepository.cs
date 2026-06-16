namespace RecrutamentoDiversidade.Repositories.Interfaces;

using RecrutamentoDiversidade.Models;

public interface IVagaRepository
{
    Task<(IEnumerable<Vaga> Items, long Total)> ListarAsync(
        int pagina, int tamanhoPagina, StatusVaga? status = null);

    Task<Vaga?> ObterPorIdAsync(long id);
    Task<Vaga?> ObterPorIdComUsuarioAsync(long id);
    Task<Vaga> CriarAsync(Vaga vaga);
    Task<Vaga> AtualizarAsync(Vaga vaga);

    Task<IEnumerable<Vaga>> ListarAbertasAsync();
}