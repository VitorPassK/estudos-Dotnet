namespace RecrutamentoDiversidade.ViewModels.Response;

using RecrutamentoDiversidade.Models;

public class CandidatoResponse
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Linkedin { get; set; }
    public string? CurriculoUrl { get; set; }
    public Genero? Genero { get; set; }
    public RacaEtnia? RacaEtnia { get; set; }
    public bool PessoaComDeficiencia { get; set; }
    public string? TipoDeficiencia { get; set; }

    public bool PertenceAGrupoSubRepresentado { get; set; }
    public DateTime CriadoEm { get; set; }

    public static CandidatoResponse FromEntity(Candidato c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Email = c.Email,
        Telefone = c.Telefone,
        Linkedin = c.Linkedin,
        CurriculoUrl = c.CurriculoUrl,
        Genero = c.Genero,
        RacaEtnia = c.RacaEtnia,
        PessoaComDeficiencia = c.PessoaComDeficiencia,
        TipoDeficiencia = c.TipoDeficiencia,
        PertenceAGrupoSubRepresentado = c.PertenceAGrupoSubRepresentado(),
        CriadoEm = c.CriadoEm
    };
}