using System.Text.Json.Serialization;

namespace insurance_policy_api.DTOs;

public record class InstallmentDetailsDTO : InstallmentDTO
{
    [JsonPropertyName("dataPago")]
    [JsonPropertyOrderAttribute(5)]
    public DateTime? PaidDate { get; set; } = null;
    
    [JsonPropertyName("juros")]
    [JsonPropertyOrderAttribute(6)]
    public decimal? Fees { get; set; } = null;

    [JsonPropertyName("situacao")]
    [JsonPropertyOrderAttribute(7)]
    public string? Status { get; set; } = null;

    [JsonPropertyName("dataCriacao")]
    [JsonPropertyOrderAttribute(8)]
    public DateTime? RecordCreationDate { get; init; }

    [JsonPropertyName("dataAlteracao")]
    [JsonPropertyOrderAttribute(9)]
    public DateTime? RecordModificationDate { get; init; }

    [JsonPropertyName("usuarioCriacao")]
    [JsonPropertyOrderAttribute(10)]
    public int? RecordCreatedByUser { get; init; }

    [JsonPropertyName("usuarioAlteracao")]
    [JsonPropertyOrderAttribute(11)]
    public int? RecordModifiedByUser { get; init; }
}
