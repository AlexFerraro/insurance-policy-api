using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace insurance_policy_api.DTOs;

public record class InstallmentDTO
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("premio")]
    [Required(ErrorMessage = "O campo premio não pode ser nulo!")]
    public decimal Premium { get; init; }

    [JsonPropertyName("formaPagamento")]
    [Required(ErrorMessage = "O campo formaPagamento não pode ser nulo ou vazio!", AllowEmptyStrings = false)]
    [RegularExpression("^(CARTAO|BOLETO|DINHEIRO)$", ErrorMessage = "O campo situacao pode conter apenas as palavras CARTAO, BOLETO e DINHEIRO!")]
    // Trocar o RegularExpression para [AllowedValues("CARTAO", "BOLETO", "DINHEIRO")] no .NET 8.
    public string PaymentMethod { get; init; }

    [JsonPropertyName("dataPagamento")]
    public DateTime PaymentDate { get; init; } = DateTime.Now;
}
