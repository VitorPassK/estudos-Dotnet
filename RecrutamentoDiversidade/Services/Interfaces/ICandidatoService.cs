namespace RecrutamentoDiversidade.Services.Interfaces;

using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public interface ICandidatoService
{
    Task<PagedResponse<CandidatoResponse>> ListarAsync(int pagina, int tamanhoPagina);
    Task<CandidatoResponse> ObterPorIdAsync(long id);
    Task<CandidatoResponse> CadastrarAsync(CandidatoRequest request);
    Task<CandidatoResponse> AtualizarAsync(long id, CandidatoRequest request);
}