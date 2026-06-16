namespace RecrutamentoDiversidade.Services.Interfaces;

using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public interface IVagaService
{
    Task<PagedResponse<VagaResponse>> ListarAsync(int pagina, int tamanhoPagina, string? status);
    Task<VagaResponse> ObterPorIdAsync(long id);
    Task<VagaResponse> CriarAsync(VagaRequest request, string emailUsuario);
    Task<VagaResponse> AtualizarAsync(long id, VagaRequest request);
    Task EncerrarAsync(long id);
}