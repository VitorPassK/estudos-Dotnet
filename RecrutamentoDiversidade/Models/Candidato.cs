namespace RecrutamentoDiversidade.Models;

public class Candidato
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string? Linkedin { get; set; }

    public string? CurriculoUrl { get; set; }

    public Genero? Genero { get; set; }

    public RacaEtnia? RacaEtnia { get; set; }

    public bool PessoaComDeficiencia { get; set; } = false;

    public string? TipoDeficiencia { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();

    public bool PertenceAGrupoSubRepresentado()
    {
        var racaNaoBranca = RacaEtnia.HasValue
            && RacaEtnia != Models.RacaEtnia.Branco
            && RacaEtnia != Models.RacaEtnia.PrefiroNaoInformar;

        var generoMinoritario = Genero.HasValue
            && (Genero == Models.Genero.Feminino || Genero == Models.Genero.NaoBinario);

        return racaNaoBranca || generoMinoritario || PessoaComDeficiencia;
    }
}

public enum Genero
{
    Masculino,
    Feminino,
    NaoBinario,
    PrefiroNaoInformar
}

public enum RacaEtnia
{
    Branco,
    Preto,
    Pardo,
    Indigena,
    Amarelo,
    PrefiroNaoInformar
}