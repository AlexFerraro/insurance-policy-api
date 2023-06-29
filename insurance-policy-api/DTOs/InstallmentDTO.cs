using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace insurance_policy_api.DTOs;

public record class InstallmentDTO
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("premio")]
    [Required(ErrorMessage = "The premium field cannot be null.")]
    public decimal Premium { get; init; }

    [JsonPropertyName("formaPagamento")]
    [Required(ErrorMessage = "The paymentMethod field cannot be null or empty.", AllowEmptyStrings = false)]
    [RegularExpression("^(CARTAO|BOLETO|DINHEIRO)$", ErrorMessage = "The paymentMethod field can only contain the words CARTAO, BOLETO and DINHEIRO.")]
    // Trocar o RegularExpression para [AllowedValues("CARTAO", "BOLETO", "DINHEIRO")] no .NET 8.
    public string PaymentMethod { get; init; }

    [JsonPropertyName("dataPagamento")]
    public DateTime PaymentDate { get; init; } = DateTime.Now;
}
