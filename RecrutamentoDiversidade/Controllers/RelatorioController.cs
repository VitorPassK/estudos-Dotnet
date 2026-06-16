namespace RecrutamentoDiversidade.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecrutamentoDiversidade.Services.Interfaces;
using RecrutamentoDiversidade.ViewModels.Response;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "RH,Gestor")]
public class RelatorioController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatorioController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("diversidade/{vagaId}")]
    [ProducesResponseType(typeof(RelatorioDiversidadeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RelatorioPorVaga(long vagaId)
    {
        var response = await _relatorioService.GerarRelatorioPorVagaAsync(vagaId);
        return Ok(response);
    }

    [HttpGet("alertas")]
    [ProducesResponseType(typeof(PagedResponse<AlertaDiversidadeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Alertas(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10)
    {
        if (pagina < 1) pagina = 1;
        if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

        var response = await _relatorioService.ListarAlertasAsync(pagina, tamanhoPagina);
        return Ok(response);
    }
}