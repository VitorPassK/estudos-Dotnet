namespace RecrutamentoDiversidade.ViewModels.Response;

public class RelatorioDiversidadeResponse
{
    public long VagaId { get; set; }
    public string TituloVaga { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public decimal MetaDiversidadePct { get; set; }

    public long TotalInscritos { get; set; }
    public long TotalPrioritarios { get; set; }
    public double PercentualDiversidadeAtual { get; set; }

    public long TotalFeminino { get; set; }
    public long TotalNaoBinario { get; set; }
    public long TotalPretoPardoIndigena { get; set; }
    public long TotalPcd { get; set; }

    public bool AbaixoDaMeta { get; set; }
    public double DiferencaParaMeta { get; set; }

    public long TotalAprovados { get; set; }
    public long AprovadosPrioritarios { get; set; }
    public double PercentualDiversidadeAprovados { get; set; }
}

public class AlertaDiversidadeResponse
{
    public long VagaId { get; set; }
    public string TituloVaga { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public decimal MetaDiversidadePct { get; set; }
    public double PercentualAtual { get; set; }
    public double DiferencaParaMeta { get; set; }
    public long TotalInscritos { get; set; }
    public long TotalPrioritarios { get; set; }
}


public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public long TotalRegistros { get; set; }
    public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanhoPagina);
    public bool TemProxima => Pagina < TotalPaginas;
    public bool TemAnterior => Pagina > 1;

    public static PagedResponse<T> Criar(IEnumerable<T> data, int pagina,
                                          int tamanhoPagina, long totalRegistros) => new()
                                          {
                                              Data = data,
                                              Pagina = pagina,
                                              TamanhoPagina = tamanhoPagina,
                                              TotalRegistros = totalRegistros
                                          };
}

public class ErrorResponse
{
    public int Status { get; set; }
    public string Erro { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public IEnumerable<string>? Erros { get; set; }
}