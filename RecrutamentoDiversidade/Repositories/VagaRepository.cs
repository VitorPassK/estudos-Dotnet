namespace RecrutamentoDiversidade.Repositories;

using Microsoft.EntityFrameworkCore;
using RecrutamentoDiversidade.Data;
using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;

public class VagaRepository : IVagaRepository
{
    private readonly AppDbContext _context;

    public VagaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Vaga> Items, long Total)> ListarAsync(
        int pagina, int tamanhoPagina, StatusVaga? status = null)
    {
        var query = _context.Vagas
            .Include(v => v.Usuario)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(v => v.Status == status.Value);

        var total = await query.LongCountAsync();

        var items = await query
            .OrderByDescending(v => v.CriadoEm)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Vaga?> ObterPorIdAsync(long id)
        => await _context.Vagas.FindAsync(id);

    public async Task<Vaga?> ObterPorIdComUsuarioAsync(long id)
        => await _context.Vagas
            .Include(v => v.Usuario)
            .FirstOrDefaultAsync(v => v.Id == id);

    public async Task<Vaga> CriarAsync(Vaga vaga)
    {
        _context.Vagas.Add(vaga);
        await _context.SaveChangesAsync();
        return vaga;
    }

    public async Task<Vaga> AtualizarAsync(Vaga vaga)
    {
        _context.Vagas.Update(vaga);
        await _context.SaveChangesAsync();
        return vaga;
    }

    public async Task<IEnumerable<Vaga>> ListarAbertasAsync()
        => await _context.Vagas
            .Where(v => v.Status == StatusVaga.Aberta)
            .Include(v => v.Candidaturas)
            .ToListAsync();
}