namespace RecrutamentoDiversidade.ViewModels.Request;

using System.ComponentModel.DataAnnotations;
using RecrutamentoDiversidade.Models;

public class CandidaturaRequest
{
    [Required(ErrorMessage = "ID do candidato é obrigatório")]
    public long CandidatoId { get; set; }

    [Required(ErrorMessage = "ID da vaga é obrigatório")]
    public long VagaId { get; set; }
}

public class AvancarEtapaRequest
{
    [Required(ErrorMessage = "Novo status é obrigatório")]
    public StatusCandidatura NovoStatus { get; set; }

    [MaxLength(2000, ErrorMessage = "Feedback deve ter no máximo 2000 caracteres")]
    public string? Feedback { get; set; }
}