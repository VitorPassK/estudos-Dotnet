namespace RecrutamentoDiversidade.Models;

public class Vaga
{
    public long Id { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public string Departamento { get; set; } = string.Empty;

    public string? LocalTrabalho { get; set; }

    public TipoContrato TipoContrato { get; set; }

    public StatusVaga Status { get; set; } = StatusVaga.Aberta;

    public decimal MetaDiversidadePct { get; set; } = 30m;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public DateTime? EncerradoEm { get; set; }

    public long UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();

    public bool EstaAberta() => Status == StatusVaga.Aberta;
}

public enum TipoContrato
{
    CLT,
    PJ,
    Estagio
}

public enum StatusVaga
{
    Aberta,
    Encerrada,
    Pausada
}