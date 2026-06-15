namespace RecrutamentoDiversidade.Models;

public class Candidatura
{
    public long Id { get; set; }
    public long CandidatoId { get; set; }
    public long VagaId { get; set; }

    public StatusCandidatura Status { get; set; } = StatusCandidatura.Inscrito;

    public bool PrioridadeDiversidade { get; set; } = false;

    public string? Observacoes { get; set; }

    public DateTime InscritoEm { get; set; } = DateTime.UtcNow;

    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;

    public Candidato? Candidato { get; set; }
    public Vaga? Vaga { get; set; }
    public void AvancarEtapa(StatusCandidatura novoStatus, string? feedback = null)
    {
        Status = novoStatus;
        AtualizadoEm = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(feedback))
            Observacoes = feedback;
    }

    public bool EstaFinalizada() =>
        Status is StatusCandidatura.Aprovado
               or StatusCandidatura.Reprovado
               or StatusCandidatura.Desistiu;
}

public enum StatusCandidatura
{
    Inscrito,
    EmTriagem,
    EntrevistaRH,
    EntrevistaTecnica,
    Proposta,
    Aprovado,
    Reprovado,
    Desistiu
}