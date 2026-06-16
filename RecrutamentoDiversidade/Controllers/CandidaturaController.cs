namespace RecrutamentoDiversidade.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CandidaturaController : ControllerBase
{
    private readonly ICandidaturaService _candidaturaService;

    public CandidaturaController(ICandidaturaService candidaturaService)
    {
        _candidaturaService = candidaturaService;
    }

    [HttpGet("vaga/{vagaId}")]
    [ProducesResponseType(typeof(IEnumerable<CandidaturaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorVaga(long vagaId)
    {
        var response = await _candidaturaService.ListarPorVagaAsync(vagaId);
        return Ok(response);
    }

    [HttpGet("candidato/{candidatoId}")]
    [ProducesResponseType(typeof(IEnumerable<CandidaturaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorCandidato(long candidatoId)
    {
        var response = await _candidaturaService.ListarPorCandidatoAsync(candidatoId);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CandidaturaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Inscrever([FromBody] CandidaturaRequest request)
    {
        var response = await _candidaturaService.InscreverAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPut("{id}/etapa")]
    [Authorize(Roles = "RH,Gestor")]
    [ProducesResponseType(typeof(CandidaturaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AvancarEtapa(long id, [FromBody] AvancarEtapaRequest request)
    {
        var emailUsuario = User.Identity!.Name
            ?? throw new UnauthorizedAccessException("Usuário não identificado");

        var response = await _candidaturaService.AvancarEtapaAsync(id, request, emailUsuario);
        return Ok(response);
    }
}