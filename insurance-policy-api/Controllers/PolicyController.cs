using insurance_policy_api.DTOs;
using insurance_policy_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace insurance_policy_api.Controllers;

[ApiController]
[Route("v1/api/apolice")]
[Produces(Application.Json)]
[Authorize]
public class PolicyController : ControllerBase
{
    private readonly IPolicyAppService _policyAppService;

    public PolicyController(IPolicyAppService policyAppService) =>
        _policyAppService = policyAppService;

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
    public async Task<IActionResult> CreatePolicyAsync([FromBody][Required] PolicyDTO policy)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var policyCreated = await _policyAppService.CreatePolicyAsync(policy);

        var urlBase = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = policyCreated,
            Links = new LinkDTO[]
            {
                new LinkDTO($"{urlBase}/{policyCreated.Id}", "get_policy", "GET"),
                new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
                new LinkDTO($"{urlBase}", "update_policy", "PATCH"),
                new LinkDTO($"{urlBase}/parcela/{policyCreated.Id}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
            }
        };

        return Created($"{urlBase}/{response.Data.Id}", response);
    }

    /// <summary>
    /// Busca todas as apólices registradas.
    /// </summary>
    /// <remarks>
    /// Retorna todas as apólices do banco de dados criadas.<br />
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDTO<IEnumerable<PolicyDetailsDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPoliciesAsync([FromQuery][Required] int skip, [FromQuery][Required] int take)
    {
        var policiesReceived = await _policyAppService.GetAllPoliciesAsync(skip, take);

        var urlBase = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var response = new ResponseDTO<IEnumerable<PolicyDetailsDTO>>()
        {
            Data = policiesReceived,
            Links = new LinkDTO[]
            {
                new LinkDTO($"{urlBase}/{policiesReceived.First().Id}", "get_first_policy", "GET"),
                new LinkDTO($"{urlBase}/{policiesReceived.Last().Id}", "get_last_policy", "GET")
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Busca uma apólice registrada.
    /// </summary>
    /// <remarks>
    /// Busca uma apólice por id e suas respectivas parcelas.<br />
    /// </remarks>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ResponseDTO<PolicyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPolicyByIdAsync([FromRoute][Required] int id)
    {
        var policyReceived = await _policyAppService.GetPolicyByIdAsync(id);

        if (policyReceived is null)
            return NotFound();

        var urlBase = $"{Request.Scheme}://{Request.Host}{Request.Path.ToString().Substring(0, 15)}";

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = policyReceived,
            Links = new LinkDTO[]
            {
                new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
                new LinkDTO($"{urlBase}", "update_policy", "PATCH"),
                new LinkDTO($"{urlBase}/parcela/{policyReceived.Id}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Atualiza uma apólice e parcelas.
    /// </summary>
    /// <remarks>
    /// Atualiza uma apólice e suas respectivas parcelas no banco de dados.
    /// </remarks>
    [HttpPatch]
    [ProducesResponseType(typeof(ResponseDTO<PolicyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePolicyAsync([FromBody][Required] PolicyDTO policy)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (policy.Id is 0)
            return BadRequest("O ID da apólice não pode ser zero.");

        var updatedPolicy = await _policyAppService.UpdatePolicyAsync(policy);

        var urlBase = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = updatedPolicy,
            Links = new LinkDTO[]
            {
                new LinkDTO($"{urlBase}/{updatedPolicy.Id}", "get_policy", "GET"),
                new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
                new LinkDTO($"{urlBase}/parcela/{updatedPolicy.Id}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
            }
        };

        return Ok(response);
    }
}
