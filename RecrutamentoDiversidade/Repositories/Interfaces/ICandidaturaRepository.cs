namespace RecrutamentoDiversidade.Repositories.Interfaces;

using RecrutamentoDiversidade.Models;

public interface ICandidaturaRepository
{
    Task<IEnumerable<Candidatura>> ListarPorVagaAsync(long vagaId);
    Task<IEnumerable<Candidatura>> ListarPorCandidatoAsync(long candidatoId);
    Task<Candidatura?> ObterPorIdAsync(long id);
    Task<bool> ExistePorCandidatoEVagaAsync(long candidatoId, long vagaId);
    Task<Candidatura> CriarAsync(Candidatura candidatura);
    Task<Candidatura> AtualizarAsync(Candidatura candidatura);

    Task<long> ContarPorVagaAsync(long vagaId);
    Task<long> ContarPrioritariosPorVagaAsync(long vagaId);
    Task<long> ContarAprovadosPorVagaAsync(long vagaId);
    Task<long> ContarAprovadosPrioritariosPorVagaAsync(long vagaId);
}