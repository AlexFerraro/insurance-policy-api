using insurance_policy_api.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace insurance_policy_api.Controllers;

[ApiController]
[Route("v1/api/apolice")]
[Produces(Application.Json)]
public class PolicyController : ControllerBase
{
    /// <summary>
    /// Cria uma apólice.
    /// </summary>
    /// <remarks>
    /// Cria uma apólice com suas parcelas de pagamento.<br />
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(PolicyDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePolicy([FromBody][Required] PolicyDTO policy)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Busca todas as apólices registradas.
    /// </summary>
    /// <remarks>
    /// Retrona todas as apólices do banco de dados criadas.<br />
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PolicyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConsultAllPolices()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Busca uma a apólice registrada.
    /// </summary>
    /// <remarks>
    /// Busca uma apólice por id e suas respectivas parcelas<br />
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(PolicyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{id:int}")]
    public async Task<IActionResult> ConsultPolicyByID([FromRoute][Required] int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Atualiza uma apólice e parcelas.
    /// </summary>
    /// <remarks>
    /// Atualiza uma apólice e suas respectivas parcelas no banco de dados.
    /// </remarks>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePolicy([FromBody][Required] PolicyDTO policy)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Lança um pagamento da apólice.
    /// </summary>
    /// <remarks>
    /// Gera a baixa de uma parcela do pagamento de uma apólice.
    /// </remarks>
    [HttpPost]
    [Route("{id:int}/pagamento")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterPayment([FromRoute][Required] int id, [FromQuery][Required] DateTime datePagamento)
    {
        throw new NotImplementedException();
    }
}