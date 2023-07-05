using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace insurance_policy_api.DTOs;

public record class PolicyBaseDTO
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("descricao")]
    [Required(ErrorMessage = "The description field cannot be null or empty.", AllowEmptyStrings = false)]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("cpf")]
    [Required(ErrorMessage = "The cpf field cannot be null.")]
    public long Cpf { get; init; }

    [JsonPropertyName("situacao")]
    [Required(ErrorMessage = "The status field cannot be null or empty.", AllowEmptyStrings = false)]
    [RegularExpression("^(ATIVA|INATIVA)$", ErrorMessage = "The status field can only contain the words ATIVA and INATIVA.")]
    // Trocar o RegularExpression para [AllowedValues("ATIVA", "INATIVA")] no .NET 8.
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("premioTotal")]
    [Required(ErrorMessage = "The premiumTotal field cannot be null.")]
    public decimal PremiumTotal { get; init; }
}
