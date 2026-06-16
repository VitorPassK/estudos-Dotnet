namespace RecrutamentoDiversidade.Services.Interfaces;

using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public interface IRelatorioService
{
    Task<RelatorioDiversidadeResponse> GerarRelatorioPorVagaAsync(long vagaId);
    Task<PagedResponse<AlertaDiversidadeResponse>> ListarAlertasAsync(int pagina, int tamanhoPagina);
}