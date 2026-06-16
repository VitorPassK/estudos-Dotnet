namespace RecrutamentoDiversidade.ViewModels.Response;

using RecrutamentoDiversidade.Models;

public class CandidaturaResponse
{
    public long Id { get; set; }
    public long CandidatoId { get; set; }
    public string NomeCandidato { get; set; } = string.Empty;
    public long VagaId { get; set; }
    public string TituloVaga { get; set; } = string.Empty;
    public StatusCandidatura Status { get; set; }
    public bool PrioridadeDiversidade { get; set; }
    public string? Observacoes { get; set; }
    public DateTime InscritoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    public static CandidaturaResponse FromEntity(Candidatura c) => new()
    {
        Id = c.Id,
        CandidatoId = c.CandidatoId,
        NomeCandidato = c.Candidato?.Nome ?? string.Empty,
        VagaId = c.VagaId,
        TituloVaga = c.Vaga?.Titulo ?? string.Empty,
        Status = c.Status,
        PrioridadeDiversidade = c.PrioridadeDiversidade,
        Observacoes = c.Observacoes,
        InscritoEm = c.InscritoEm,
        AtualizadoEm = c.AtualizadoEm
    };
}