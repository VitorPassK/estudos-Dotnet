namespace RecrutamentoDiversidade.ViewModels.Request;

using System.ComponentModel.DataAnnotations;
using RecrutamentoDiversidade.Models;

public class CandidatoRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefone { get; set; }

    [MaxLength(255)]
    public string? Linkedin { get; set; }

    [MaxLength(500)]
    public string? CurriculoUrl { get; set; }

    public Genero? Genero { get; set; }
    public RacaEtnia? RacaEtnia { get; set; }
    public bool PessoaComDeficiencia { get; set; } = false;

    [MaxLength(100)]
    public string? TipoDeficiencia { get; set; }
}