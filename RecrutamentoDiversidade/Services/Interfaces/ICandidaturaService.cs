namespace RecrutamentoDiversidade.Services.Interfaces;

using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public interface ICandidaturaService
{
    Task<IEnumerable<CandidaturaResponse>> ListarPorVagaAsync(long vagaId);
    Task<IEnumerable<CandidaturaResponse>> ListarPorCandidatoAsync(long candidatoId);
    Task<CandidaturaResponse> InscreverAsync(CandidaturaRequest request);
    Task<CandidaturaResponse> AvancarEtapaAsync(long id, AvancarEtapaRequest request, string emailUsuario);
}