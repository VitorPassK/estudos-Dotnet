namespace RecrutamentoDiversidade.Repositories;

using Microsoft.EntityFrameworkCore;
using RecrutamentoDiversidade.Data;
using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;

public class CandidaturaRepository : ICandidaturaRepository
{
    private readonly AppDbContext _context;

    public CandidaturaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Candidatura>> ListarPorVagaAsync(long vagaId)
        => await _context.Candidaturas
            .Include(c => c.Candidato)
            .Include(c => c.Vaga)
            .Where(c => c.VagaId == vagaId)
            .ToListAsync();

    public async Task<IEnumerable<Candidatura>> ListarPorCandidatoAsync(long candidatoId)
        => await _context.Candidaturas
            .Include(c => c.Candidato)
            .Include(c => c.Vaga)
            .Where(c => c.CandidatoId == candidatoId)
            .ToListAsync();

    public async Task<Candidatura?> ObterPorIdAsync(long id)
        => await _context.Candidaturas
            .Include(c => c.Candidato)
            .Include(c => c.Vaga)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExistePorCandidatoEVagaAsync(long candidatoId, long vagaId)
        => await _context.Candidaturas
            .AnyAsync(c => c.CandidatoId == candidatoId && c.VagaId == vagaId);

    public async Task<Candidatura> CriarAsync(Candidatura candidatura)
    {
        _context.Candidaturas.Add(candidatura);
        await _context.SaveChangesAsync();
        return candidatura;
    }

    public async Task<Candidatura> AtualizarAsync(Candidatura candidatura)
    {
        _context.Candidaturas.Update(candidatura);
        await _context.SaveChangesAsync();
        return candidatura;
    }

    public async Task<long> ContarPorVagaAsync(long vagaId)
        => await _context.Candidaturas.LongCountAsync(c => c.VagaId == vagaId);

    public async Task<long> ContarPrioritariosPorVagaAsync(long vagaId)
        => await _context.Candidaturas
            .LongCountAsync(c => c.VagaId == vagaId && c.PrioridadeDiversidade);

    public async Task<long> ContarAprovadosPorVagaAsync(long vagaId)
        => await _context.Candidaturas
            .LongCountAsync(c => c.VagaId == vagaId
                              && c.Status == StatusCandidatura.Aprovado);

    public async Task<long> ContarAprovadosPrioritariosPorVagaAsync(long vagaId)
        => await _context.Candidaturas
            .LongCountAsync(c => c.VagaId == vagaId
                              && c.Status == StatusCandidatura.Aprovado
                              && c.PrioridadeDiversidade);
}