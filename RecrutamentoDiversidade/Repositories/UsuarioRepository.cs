namespace RecrutamentoDiversidade.Repositories;

using Microsoft.EntityFrameworkCore;
using RecrutamentoDiversidade.Data;
using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> ExistePorEmailAsync(string email)
        => await _context.Usuarios.AnyAsync(u => u.Email == email);

    public async Task<Usuario> CriarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}