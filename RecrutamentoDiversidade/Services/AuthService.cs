namespace RecrutamentoDiversidade.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("E-mail ou senha inválidos");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário inativo");

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha))
            throw new UnauthorizedAccessException("E-mail ou senha inválidos");

        return GerarAuthResponse(usuario);
    }

    public async Task<AuthResponse> RegistroAsync(RegistroRequest request)
    {
        if (await _usuarioRepository.ExistePorEmailAsync(request.Email))
            throw new InvalidOperationException($"E-mail já cadastrado: {request.Email}");

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Role = request.Role,
            Ativo = true
        };

        await _usuarioRepository.CriarAsync(usuario);
        return GerarAuthResponse(usuario);
    }

    private AuthResponse GerarAuthResponse(Usuario usuario)
    {
        var secret = _configuration["Jwt:Secret"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var horasExp = int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24");
        var expiracao = DateTime.UtcNow.AddHours(horasExp);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   usuario.Email),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name,               usuario.Nome),
            new Claim(ClaimTypes.Role,               usuario.Role.ToString()),
            new Claim("userId",                      usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiracao,
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Tipo = "Bearer",
            Nome = usuario.Nome,
            Email = usuario.Email,
            Role = usuario.Role.ToString(),
            Expiracao = expiracao
        };
    }
}