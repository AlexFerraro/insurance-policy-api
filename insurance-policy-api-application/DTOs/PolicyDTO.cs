using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace insurance_policy_api_application.DTOs;

public record class PolicyDTO : PolicyBaseDTO
{
    [JsonPropertyName("parcelas")]
    [JsonPropertyOrderAttribute(6)]
    [MinLength(1, ErrorMessage = "The policy needs to have at least one installment in order to be created.")]
    public IEnumerable<InstallmentDTO>? Installments { get; init; }
}
