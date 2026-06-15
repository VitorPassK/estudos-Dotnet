namespace RecrutamentoDiversidade.ViewModels.Response;

using RecrutamentoDiversidade.Models;

public class VagaResponse
{
    public long Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Departamento { get; set; } = string.Empty;
    public string? LocalTrabalho { get; set; }
    public TipoContrato TipoContrato { get; set; }
    public StatusVaga Status { get; set; }
    public decimal MetaDiversidadePct { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? EncerradoEm { get; set; }

    public string NomeResponsavel { get; set; } = string.Empty;

    public static VagaResponse FromEntity(Vaga vaga) => new()
    {
        Id = vaga.Id,
        Titulo = vaga.Titulo,
        Descricao = vaga.Descricao,
        Departamento = vaga.Departamento,
        LocalTrabalho = vaga.LocalTrabalho,
        TipoContrato = vaga.TipoContrato,
        Status = vaga.Status,
        MetaDiversidadePct = vaga.MetaDiversidadePct,
        CriadoEm = vaga.CriadoEm,
        EncerradoEm = vaga.EncerradoEm,
        NomeResponsavel = vaga.Usuario?.Nome ?? string.Empty
    };
}