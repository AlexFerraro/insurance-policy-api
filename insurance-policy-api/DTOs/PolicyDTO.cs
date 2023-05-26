using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace insurance_policy_api.DTOs;

public record class PolicyDTO : PolicyBaseDTO
{
    [JsonPropertyName("parcelas")]
    [JsonPropertyOrderAttribute(6)]
    [MinLength(1, ErrorMessage = "A apólice necessita ter ao menos uma parcela!")]
    public IEnumerable<InstallmentDTO>? Installments { get; init; }
}
