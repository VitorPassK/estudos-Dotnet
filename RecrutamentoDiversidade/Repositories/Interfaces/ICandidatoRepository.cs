namespace RecrutamentoDiversidade.Repositories.Interfaces;

using RecrutamentoDiversidade.Models;
public interface ICandidatoRepository
{
    Task<(IEnumerable<Candidato> Items, long Total)> ListarAsync(
        int pagina, int tamanhoPagina);

    Task<Candidato?> ObterPorIdAsync(long id);
    Task<Candidato?> ObterPorEmailAsync(string email);
    Task<bool> ExistePorEmailAsync(string email);
    Task<Candidato> CriarAsync(Candidato candidato);
    Task<Candidato> AtualizarAsync(Candidato candidato);

    Task<IEnumerable<Candidato>> ListarPorVagaAsync(long vagaId);
}