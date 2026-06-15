namespace RecrutamentoDiversidade.ViewModels.Request;

using System.ComponentModel.DataAnnotations;
using RecrutamentoDiversidade.Models;

public class VagaRequest
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [MaxLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Departamento é obrigatório")]
    [MaxLength(100, ErrorMessage = "Departamento deve ter no máximo 100 caracteres")]
    public string Departamento { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? LocalTrabalho { get; set; }

    [Required(ErrorMessage = "Tipo de contrato é obrigatório")]
    public TipoContrato TipoContrato { get; set; }

    [Required(ErrorMessage = "Meta de diversidade é obrigatória")]
    [Range(0, 100, ErrorMessage = "Meta deve estar entre 0 e 100")]
    public decimal MetaDiversidadePct { get; set; }
}