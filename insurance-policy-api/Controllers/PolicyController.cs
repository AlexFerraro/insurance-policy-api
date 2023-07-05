using insurance_policy_api.DTOs;
using insurance_policy_api.Factory;
using insurance_policy_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace insurance_policy_api.Controllers;

[ApiController]
[Route("v1/api/apolice")]
[Produces(Application.Json)]
public class PolicyController : ControllerBase
{
    private readonly IPolicyAppService _policyAppService;
    private readonly HateoasFactory _hateoasFactory;

    public PolicyController(IPolicyAppService policyAppService, HateoasFactory hateoasFactory) =>
        (_policyAppService, _hateoasFactory) = (policyAppService, hateoasFactory);

    /// <summary>
    /// Creates a policy.
    /// </summary>
    /// <remarks>
    /// This method creates a policy with its payment installments. <br />
    /// Make sure to provide all the necessary details to create the policy correctly. <br />
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDTO<PolicyDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePolicyAsync([FromBody][Required] PolicyDTO policy)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (policy.Installments is null)
            return BadRequest("The policy needs to have at least one installment in order to be created.");

        var policyCreated = await _policyAppService.CreatePolicyAsync(policy);

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = policyCreated,
            Links = _hateoasFactory.CreateLinksForCreatedPolicy(policyCreated.Id, GetBaseUrl())
        };

        return Created($"{GetBaseUrl()}/{response.Data.Id}", response);
    }

    /// <summary>
    /// Retrieves all registered policies.
    /// </summary>
    /// <remarks>
    /// This method retrieves all registered policies from the database in a paginated manner. <br />
    /// Make sure to correctly specify the pagination parameters to obtain the desired results. <br />
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDTO<IEnumerable<PolicyDetailsDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPoliciesAsync([FromQuery][Required] int skip, [FromQuery][Required] int take)
    {
        var policiesReceived = await _policyAppService.GetAllPoliciesAsync(skip, take);

        var response = new ResponseDTO<IEnumerable<PolicyDetailsDTO>>()
        {
            Data = policiesReceived,
            Links = _hateoasFactory.CreateLinksForGetAllPolicies(GetBaseUrl())
        };

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a registered policy.
    /// </summary>
    /// <remarks>
    /// Retrieves a policy by its ID and its respective payment installments. <br />
    /// Make sure to provide the correct policy ID to obtain the desired information. <br />
    /// </remarks>
    [HttpGet("{long:int}")]
    [ProducesResponseType(typeof(ResponseDTO<PolicyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPolicyByIdAsync([FromRoute][Required] long id)
    {
        var policyReceived = await _policyAppService.GetPolicyByIdAsync(id);

        if (policyReceived is null)
            return NotFound("The policy was not found.");

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = policyReceived,
            Links = _hateoasFactory.CreateLinksForGetPolicyById(GetBaseUrl())
        };

        return Ok(response);
    }

    /// <summary>
    /// Updates a policy and its installments.
    /// </summary>
    /// <remarks>
    /// This method updates a policy and its respective installments in the database. If the provided array of installments is null, only the policy will be updated. <br />
    /// Make sure to provide the correct data to update the policy and its installments. <br />
    /// </remarks>
    [HttpPatch]
    [ProducesResponseType(typeof(ResponseDTO<PolicyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePolicyAsync([FromBody][Required] PolicyDTO policy)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (policy.Id is 0)
            return BadRequest("The policy ID cannot be zero in payload.");

        var updatedPolicy = await _policyAppService.UpdatePolicyAsync(policy);

        var response = new ResponseDTO<PolicyDTO>()
        {
            Data = updatedPolicy,
            Links = _hateoasFactory.CreateLinksForUpdatedPolicy(updatedPolicy.Id, GetBaseUrl())
        };

        return Ok(response);
    }

    private string GetBaseUrl() =>
        $"{Request.Scheme}://{Request.Host}{Request.Path}";
}
