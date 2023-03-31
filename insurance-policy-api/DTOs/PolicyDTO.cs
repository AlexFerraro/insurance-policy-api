using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace insurance_policy_api.DTOs;

public record class PolicyDTO
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("descricao")]
    [Required(ErrorMessage = "O campo descrição não pode ser nulo ou vazio!", AllowEmptyStrings = false)]
    public string Description { get; init; }

    [JsonPropertyName("cpf")]
    [Required(ErrorMessage = "O campo cpf não pode ser nulo!")]
    public long Cpf { get; init; }

    [JsonPropertyName("situacao")]
    [Required(ErrorMessage = "O campo situacao não pode ser nulo ou vazio!", AllowEmptyStrings = false)]
    [RegularExpression("^(ATIVA|INATIVA)$", ErrorMessage = "O campo situacao pode conter apenas as palavras ATIVA e INATIVA!")]
    // Trocar o RegularExpression para [AllowedValues("ATIVA", "INATIVA")] no .NET 8.
    public string Status { get; init; }

    [JsonPropertyName("premioTotal")]
    [Required(ErrorMessage = "O campo premioTotal não pode ser nulo!")]
    public decimal PremiumTotal { get; init; }

    [JsonPropertyName("parcelas")]
    [Required(ErrorMessage = "A parcela não pode ser nula!")]
    [MinLength(1, ErrorMessage = "A apólice necessita ter ao menos uma parcela!")]
    public IEnumerable<InstallmentDTO> Installments { get; init; }
}
