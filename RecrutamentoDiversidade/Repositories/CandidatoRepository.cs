namespace RecrutamentoDiversidade.Repositories;

using Microsoft.EntityFrameworkCore;
using RecrutamentoDiversidade.Data;
using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;

public class CandidatoRepository : ICandidatoRepository
{
    private readonly AppDbContext _context;

    public CandidatoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Candidato> Items, long Total)> ListarAsync(
        int pagina, int tamanhoPagina)
    {
        var total = await _context.Candidatos.LongCountAsync();

        var items = await _context.Candidatos
            .OrderBy(c => c.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Candidato?> ObterPorIdAsync(long id)
        => await _context.Candidatos.FindAsync(id);

    public async Task<Candidato?> ObterPorEmailAsync(string email)
        => await _context.Candidatos
            .FirstOrDefaultAsync(c => c.Email == email);

    public async Task<bool> ExistePorEmailAsync(string email)
        => await _context.Candidatos.AnyAsync(c => c.Email == email);

    public async Task<Candidato> CriarAsync(Candidato candidato)
    {
        _context.Candidatos.Add(candidato);
        await _context.SaveChangesAsync();
        return candidato;
    }

    public async Task<Candidato> AtualizarAsync(Candidato candidato)
    {
        _context.Candidatos.Update(candidato);
        await _context.SaveChangesAsync();
        return candidato;
    }

    public async Task<IEnumerable<Candidato>> ListarPorVagaAsync(long vagaId)
        => await _context.Candidatos
            .Where(c => c.Candidaturas.Any(ca => ca.VagaId == vagaId))
            .ToListAsync();
}