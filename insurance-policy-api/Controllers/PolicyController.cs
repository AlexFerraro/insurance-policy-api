using insurance_policy_api.DTOs;
using insurance_policy_api_domain.Contracts;
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
    [ProducesResponseType(typeof(ResponseDTO<PolicyDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePolicyAsync([FromBody][Required] PolicyDTO policy
        , [FromServices] IPolicyDomainService policyDomainService)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await policyDomainService.CreatePolicyAsync(policy);

        var urlBase = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = policy,
            Links = new LinkDTO[]
            {
                    new LinkDTO($"{urlBase}/{policy.Id}", "get_policy", "GET"),
                    //new LinkDTO($"{urlBase}", "get_all_policy", "GET"),
                    new LinkDTO($"{urlBase}", "update_policy", "PATCH"),
                    new LinkDTO($"{urlBase}/{policy.Id}/pagamento", "register_payment", "POST")
            }
        };

       return Created($"{ urlBase }/{ response.Data.Id }", response);
    }

    /// <summary>
    /// Busca todas as apólices registradas.
    /// </summary>
    /// <remarks>
    /// Retorna todas as apólices do banco de dados criadas.<br />
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PolicyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPoliciesAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Busca uma a apólice registrada.
    /// </summary>
    /// <remarks>
    /// Busca uma apólice por id e suas respectivas parcelas.<br />
    /// </remarks>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PolicyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPolicyByIdAsync([FromRoute][Required] int id)
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
    public async Task<IActionResult> UpdatePolicyAsync([FromBody][Required] PolicyDTO policy)
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
    [HttpPost("{id:int}/pagamento")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterPaymentAsync([FromRoute][Required] int id, [FromQuery][Required] DateTime datePagamento)
    {
        throw new NotImplementedException();
    }
}
