namespace RecrutamentoDiversidade.Services;

using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public class CandidatoService : ICandidatoService
{
    private readonly ICandidatoRepository _candidatoRepository;

    public CandidatoService(ICandidatoRepository candidatoRepository)
    {
        _candidatoRepository = candidatoRepository;
    }

    public async Task<PagedResponse<CandidatoResponse>> ListarAsync(int pagina, int tamanhoPagina)
    {
        var (items, total) = await _candidatoRepository.ListarAsync(pagina, tamanhoPagina);

        return PagedResponse<CandidatoResponse>.Criar(
            items.Select(CandidatoResponse.FromEntity),
            pagina,
            tamanhoPagina,
            total
        );
    }

    public async Task<CandidatoResponse> ObterPorIdAsync(long id)
    {
        var candidato = await _candidatoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Candidato não encontrado: {id}");

        return CandidatoResponse.FromEntity(candidato);
    }

    public async Task<CandidatoResponse> CadastrarAsync(CandidatoRequest request)
    {
        if (await _candidatoRepository.ExistePorEmailAsync(request.Email))
            throw new InvalidOperationException($"E-mail já cadastrado: {request.Email}");

        if (request.PessoaComDeficiencia &&
            string.IsNullOrWhiteSpace(request.TipoDeficiencia))
            throw new InvalidOperationException(
                "Tipo de deficiência é obrigatório para pessoas com deficiência");

        var candidato = new Candidato
        {
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone,
            Linkedin = request.Linkedin,
            CurriculoUrl = request.CurriculoUrl,
            Genero = request.Genero,
            RacaEtnia = request.RacaEtnia,
            PessoaComDeficiencia = request.PessoaComDeficiencia,
            TipoDeficiencia = request.TipoDeficiencia
        };

        await _candidatoRepository.CriarAsync(candidato);
        return CandidatoResponse.FromEntity(candidato);
    }

    public async Task<CandidatoResponse> AtualizarAsync(long id, CandidatoRequest request)
    {
        var candidato = await _candidatoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Candidato não encontrado: {id}");

        if (candidato.Email != request.Email &&
            await _candidatoRepository.ExistePorEmailAsync(request.Email))
            throw new InvalidOperationException($"E-mail já cadastrado: {request.Email}");

        if (request.PessoaComDeficiencia &&
            string.IsNullOrWhiteSpace(request.TipoDeficiencia))
            throw new InvalidOperationException(
                "Tipo de deficiência é obrigatório para pessoas com deficiência");

        candidato.Nome = request.Nome;
        candidato.Email = request.Email;
        candidato.Telefone = request.Telefone;
        candidato.Linkedin = request.Linkedin;
        candidato.CurriculoUrl = request.CurriculoUrl;
        candidato.Genero = request.Genero;
        candidato.RacaEtnia = request.RacaEtnia;
        candidato.PessoaComDeficiencia = request.PessoaComDeficiencia;
        candidato.TipoDeficiencia = request.TipoDeficiencia;

        await _candidatoRepository.AtualizarAsync(candidato);
        return CandidatoResponse.FromEntity(candidato);
    }
}