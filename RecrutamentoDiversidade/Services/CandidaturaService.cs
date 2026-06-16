namespace RecrutamentoDiversidade.Services;

using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public class CandidaturaService : ICandidaturaService
{
    private readonly ICandidaturaRepository _candidaturaRepository;
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly IVagaRepository _vagaRepository;

    public CandidaturaService(
        ICandidaturaRepository candidaturaRepository,
        ICandidatoRepository candidatoRepository,
        IVagaRepository vagaRepository)
    {
        _candidaturaRepository = candidaturaRepository;
        _candidatoRepository = candidatoRepository;
        _vagaRepository = vagaRepository;
    }

    public async Task<IEnumerable<CandidaturaResponse>> ListarPorVagaAsync(long vagaId)
    {
        var candidaturas = await _candidaturaRepository.ListarPorVagaAsync(vagaId);
        return candidaturas.Select(CandidaturaResponse.FromEntity);
    }

    public async Task<IEnumerable<CandidaturaResponse>> ListarPorCandidatoAsync(long candidatoId)
    {
        var candidaturas = await _candidaturaRepository.ListarPorCandidatoAsync(candidatoId);
        return candidaturas.Select(CandidaturaResponse.FromEntity);
    }

    public async Task<CandidaturaResponse> InscreverAsync(CandidaturaRequest request)
    {
        var vaga = await _vagaRepository.ObterPorIdAsync(request.VagaId)
            ?? throw new KeyNotFoundException($"Vaga não encontrada: {request.VagaId}");

        if (!vaga.EstaAberta())
            throw new InvalidOperationException(
                $"A vaga '{vaga.Titulo}' não está aceitando candidaturas");

        var candidato = await _candidatoRepository.ObterPorIdAsync(request.CandidatoId)
            ?? throw new KeyNotFoundException($"Candidato não encontrado: {request.CandidatoId}");

        if (await _candidaturaRepository.ExistePorCandidatoEVagaAsync(
                request.CandidatoId, request.VagaId))
            throw new InvalidOperationException("Candidato já inscrito nesta vaga");

        var candidatura = new Candidatura
        {
            CandidatoId = candidato.Id,
            VagaId = vaga.Id,
            Candidato = candidato,
            Vaga = vaga,
            Status = StatusCandidatura.Inscrito,
            PrioridadeDiversidade = candidato.PertenceAGrupoSubRepresentado()
        };

        await _candidaturaRepository.CriarAsync(candidatura);
        return CandidaturaResponse.FromEntity(candidatura);
    }

    public async Task<CandidaturaResponse> AvancarEtapaAsync(
        long id, AvancarEtapaRequest request, string emailUsuario)
    {
        var candidatura = await _candidaturaRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Candidatura não encontrada: {id}");

        if (candidatura.EstaFinalizada())
            throw new InvalidOperationException(
                $"Candidatura já finalizada com status: {candidatura.Status}");

        candidatura.AvancarEtapa(request.NovoStatus, request.Feedback);

        await _candidaturaRepository.AtualizarAsync(candidatura);
        return CandidaturaResponse.FromEntity(candidatura);
    }
}

public class RelatorioService : IRelatorioService
{
    private readonly IVagaRepository _vagaRepository;
    private readonly ICandidaturaRepository _candidaturaRepository;
    private readonly ICandidatoRepository _candidatoRepository;

    public RelatorioService(
        IVagaRepository vagaRepository,
        ICandidaturaRepository candidaturaRepository,
        ICandidatoRepository candidatoRepository)
    {
        _vagaRepository = vagaRepository;
        _candidaturaRepository = candidaturaRepository;
        _candidatoRepository = candidatoRepository;
    }

    public async Task<RelatorioDiversidadeResponse> GerarRelatorioPorVagaAsync(long vagaId)
    {
        var vaga = await _vagaRepository.ObterPorIdComUsuarioAsync(vagaId)
            ?? throw new KeyNotFoundException($"Vaga não encontrada: {vagaId}");

        var totalInscritos = await _candidaturaRepository.ContarPorVagaAsync(vagaId);
        var totalPrioritarios = await _candidaturaRepository.ContarPrioritariosPorVagaAsync(vagaId);
        var totalAprovados = await _candidaturaRepository.ContarAprovadosPorVagaAsync(vagaId);
        var aprovadosPrioritarios = await _candidaturaRepository.ContarAprovadosPrioritariosPorVagaAsync(vagaId);

        var candidatos = await _candidatoRepository.ListarPorVagaAsync(vagaId);

        long totalFeminino = candidatos.Count(c => c.Genero == Models.Genero.Feminino);
        long totalNaoBinario = candidatos.Count(c => c.Genero == Models.Genero.NaoBinario);
        long totalPretoPardoIndig = candidatos.Count(c =>
            c.RacaEtnia is Models.RacaEtnia.Preto
                        or Models.RacaEtnia.Pardo
                        or Models.RacaEtnia.Indigena);
        long totalPcd = candidatos.Count(c => c.PessoaComDeficiencia);

        double pctAtual = totalInscritos > 0
            ? Math.Round((double)totalPrioritarios / totalInscritos * 100, 2) : 0;
        double pctAprovados = totalAprovados > 0
            ? Math.Round((double)aprovadosPrioritarios / totalAprovados * 100, 2) : 0;

        double meta = (double)vaga.MetaDiversidadePct;
        double diferenca = Math.Round(pctAtual - meta, 2);

        return new RelatorioDiversidadeResponse
        {
            VagaId = vaga.Id,
            TituloVaga = vaga.Titulo,
            Departamento = vaga.Departamento,
            MetaDiversidadePct = vaga.MetaDiversidadePct,
            TotalInscritos = totalInscritos,
            TotalPrioritarios = totalPrioritarios,
            PercentualDiversidadeAtual = pctAtual,
            TotalFeminino = totalFeminino,
            TotalNaoBinario = totalNaoBinario,
            TotalPretoPardoIndigena = totalPretoPardoIndig,
            TotalPcd = totalPcd,
            AbaixoDaMeta = pctAtual < meta,
            DiferencaParaMeta = diferenca,
            TotalAprovados = totalAprovados,
            AprovadosPrioritarios = aprovadosPrioritarios,
            PercentualDiversidadeAprovados = pctAprovados
        };
    }

    public async Task<PagedResponse<AlertaDiversidadeResponse>> ListarAlertasAsync(
        int pagina, int tamanhoPagina)
    {
        var vagasAbertas = await _vagaRepository.ListarAbertasAsync();
        var alertas = new List<AlertaDiversidadeResponse>();

        foreach (var vaga in vagasAbertas)
        {
            var total = await _candidaturaRepository.ContarPorVagaAsync(vaga.Id);
            var prioritarios = await _candidaturaRepository.ContarPrioritariosPorVagaAsync(vaga.Id);

            if (total == 0) continue;

            double pct = Math.Round((double)prioritarios / total * 100, 2);
            double meta = (double)vaga.MetaDiversidadePct;

            if (pct < meta)
            {
                alertas.Add(new AlertaDiversidadeResponse
                {
                    VagaId = vaga.Id,
                    TituloVaga = vaga.Titulo,
                    Departamento = vaga.Departamento,
                    MetaDiversidadePct = vaga.MetaDiversidadePct,
                    PercentualAtual = pct,
                    DiferencaParaMeta = Math.Round(pct - meta, 2),
                    TotalInscritos = total,
                    TotalPrioritarios = prioritarios
                });
            }
        }

        var totalAlertas = alertas.Count;
        var paginados = alertas
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina);

        return PagedResponse<AlertaDiversidadeResponse>.Criar(
            paginados, pagina, tamanhoPagina, totalAlertas);
    }
}