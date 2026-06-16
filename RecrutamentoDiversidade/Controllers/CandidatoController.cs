namespace RecrutamentoDiversidade.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CandidatoController : ControllerBase
{
    private readonly ICandidatoService _candidatoService;

    public CandidatoController(ICandidatoService candidatoService)
    {
        _candidatoService = candidatoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CandidatoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10)
    {
        if (pagina < 1) pagina = 1;
        if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

        var response = await _candidatoService.ListarAsync(pagina, tamanhoPagina);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CandidatoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _candidatoService.ObterPorIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CandidatoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cadastrar([FromBody] CandidatoRequest request)
    {
        var response = await _candidatoService.CadastrarAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CandidatoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Atualizar(long id, [FromBody] CandidatoRequest request)
    {
        var response = await _candidatoService.AtualizarAsync(id, request);
        return Ok(response);
    }
}