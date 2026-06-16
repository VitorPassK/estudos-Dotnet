namespace RecrutamentoDiversidade.Services;

using RecrutamentoDiversidade.Models;
using RecrutamentoDiversidade.Repositories.Interfaces;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

public class VagaService : IVagaService
{
    private readonly IVagaRepository _vagaRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public VagaService(IVagaRepository vagaRepository, IUsuarioRepository usuarioRepository)
    {
        _vagaRepository = vagaRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<PagedResponse<VagaResponse>> ListarAsync(
        int pagina, int tamanhoPagina, string? status)
    {
        StatusVaga? statusEnum = null;
        if (!string.IsNullOrEmpty(status) &&
            Enum.TryParse<StatusVaga>(status, true, out var parsed))
            statusEnum = parsed;

        var (items, total) = await _vagaRepository.ListarAsync(pagina, tamanhoPagina, statusEnum);

        return PagedResponse<VagaResponse>.Criar(
            items.Select(VagaResponse.FromEntity),
            pagina,
            tamanhoPagina,
            total
        );
    }

    public async Task<VagaResponse> ObterPorIdAsync(long id)
    {
        var vaga = await _vagaRepository.ObterPorIdComUsuarioAsync(id)
            ?? throw new KeyNotFoundException($"Vaga não encontrada: {id}");

        return VagaResponse.FromEntity(vaga);
    }

    public async Task<VagaResponse> CriarAsync(VagaRequest request, string emailUsuario)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(emailUsuario)
            ?? throw new KeyNotFoundException("Usuário não encontrado");

        var vaga = new Vaga
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Departamento = request.Departamento,
            LocalTrabalho = request.LocalTrabalho,
            TipoContrato = request.TipoContrato,
            MetaDiversidadePct = request.MetaDiversidadePct,
            Status = StatusVaga.Aberta,
            UsuarioId = usuario.Id,
            Usuario = usuario
        };

        await _vagaRepository.CriarAsync(vaga);
        return VagaResponse.FromEntity(vaga);
    }

    public async Task<VagaResponse> AtualizarAsync(long id, VagaRequest request)
    {
        var vaga = await _vagaRepository.ObterPorIdComUsuarioAsync(id)
            ?? throw new KeyNotFoundException($"Vaga não encontrada: {id}");

        if (vaga.Status == StatusVaga.Encerrada)
            throw new InvalidOperationException("Não é possível editar uma vaga encerrada");

        vaga.Titulo = request.Titulo;
        vaga.Descricao = request.Descricao;
        vaga.Departamento = request.Departamento;
        vaga.LocalTrabalho = request.LocalTrabalho;
        vaga.TipoContrato = request.TipoContrato;
        vaga.MetaDiversidadePct = request.MetaDiversidadePct;

        await _vagaRepository.AtualizarAsync(vaga);
        return VagaResponse.FromEntity(vaga);
    }

    public async Task EncerrarAsync(long id)
    {
        var vaga = await _vagaRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Vaga não encontrada: {id}");

        if (vaga.Status == StatusVaga.Encerrada)
            throw new InvalidOperationException("Vaga já está encerrada");

        vaga.Status = StatusVaga.Encerrada;
        vaga.EncerradoEm = DateTime.UtcNow;

        await _vagaRepository.AtualizarAsync(vaga);
    }
}