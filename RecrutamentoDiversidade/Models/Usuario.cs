namespace RecrutamentoDiversidade.Models;

public class Usuario
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public RoleUsuario Role { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<Vaga> Vagas { get; set; } = new List<Vaga>();
}

public enum RoleUsuario
{
    RH,
    Gestor
}