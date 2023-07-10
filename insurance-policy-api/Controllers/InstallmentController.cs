using insurance_policy_api.DTOs;
using insurance_policy_api.Factory;
using insurance_policy_api_application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace insurance_policy_api.Controllers;

[ApiController]
[Route("v1/api/parcela")]
[Produces(Application.Json)]
public class InstallmentController : ControllerBase
{
    private readonly HateoasFactory _hateoasFactory;

    public InstallmentController(HateoasFactory hateoasFactory) =>
        _hateoasFactory = hateoasFactory;

    /// <summary>
    /// Make a payment for an installment of a policy.
    /// </summary>
    /// <remarks>
    /// Endpoint responsible for registering the payment of an installment of a policy.
    /// After the payment is registered, the corresponding installment is marked as paid.
    /// An installment that is already paid cannot be paid again.
    /// </remarks>
    [HttpPost("{long:int}/pagamento")]
    [ProducesResponseType(typeof(LinkDTO[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterPaymentAsync([FromRoute][Required] long id, [FromQuery][Required] DateTime paidDate
        , [FromServices] IInstallmentAppService installmentAppService)
    {
        await installmentAppService.RegisterPaymentForPolicyAsync(id, DateOnly.FromDateTime(paidDate));

        var response = _hateoasFactory.CreateLinksForRegisterPaymentForPolicy(GetBaseUrl());

        return Ok(response);
    }

    private string GetBaseUrl() =>
        $"{Request.Scheme}://{Request.Host}/v1/api/apolice";
}