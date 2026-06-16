namespace RecrutamentoDiversidade.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Request;
using RecrutamentoDiversidade.ViewModels.Response;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VagaController : ControllerBase
{
    private readonly IVagaService _vagaService;

    public VagaController(IVagaService vagaService)
    {
        _vagaService = vagaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<VagaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        [FromQuery] string? status = null)
    {
        if (pagina < 1) pagina = 1;
        if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

        var response = await _vagaService.ListarAsync(pagina, tamanhoPagina, status);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VagaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var response = await _vagaService.ObterPorIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "RH")]
    [ProducesResponseType(typeof(VagaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] VagaRequest request)
    {
        var emailUsuario = User.Identity!.Name
            ?? throw new UnauthorizedAccessException("Usuário não identificado");

        var response = await _vagaService.CriarAsync(request, emailUsuario);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "RH")]
    [ProducesResponseType(typeof(VagaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Atualizar(long id, [FromBody] VagaRequest request)
    {
        var response = await _vagaService.AtualizarAsync(id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "RH")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Encerrar(long id)
    {
        await _vagaService.EncerrarAsync(id);
        return NoContent();
    }
}