namespace RecrutamentoDiversidade.ViewModels.Request;

using System.ComponentModel.DataAnnotations;
using RecrutamentoDiversidade.Models;

public class RegistroRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
    [MaxLength(100, ErrorMessage = "Senha deve ter no máximo 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role é obrigatória")]
    public RoleUsuario Role { get; set; }
}