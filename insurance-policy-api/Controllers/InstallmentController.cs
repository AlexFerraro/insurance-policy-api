using insurance_policy_api.DTOs;
using insurance_policy_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace insurance_policy_api.Controllers;

[ApiController]
[Route("v1/api/parcela")]
[Produces(Application.Json)]
[Authorize]
public class InstallmentController : ControllerBase
{
    /// <summary>
    /// Lança um pagamento de uma parcela de uma apólice.
    /// </summary>
    /// <remarks>
    /// Gera a baixa de uma parcela do pagamento de uma apólice.
    /// </remarks>
    [HttpPost("{id:int}/pagamento")]
    [ProducesResponseType(typeof(LinkDTO[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterPaymentAsync([FromRoute][Required] int id, [FromQuery][Required] DateTime paidDate
                                                            , [FromServices] IInstallmentAppService _installmentAppService)
    {
        await _installmentAppService.RegisterPaymentAsync(id, DateOnly.FromDateTime(paidDate));

        var urlBase = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var response = new LinkDTO[]
        {
            new LinkDTO($"{urlBase}/{id}", "get_policy", "GET"),
            new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
            new LinkDTO($"{urlBase}", "update_policy", "PATCH")
        };

        return Ok(response);
    }
}
