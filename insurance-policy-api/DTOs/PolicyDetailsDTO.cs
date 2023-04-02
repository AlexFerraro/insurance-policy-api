using System.Text.Json.Serialization;

namespace insurance_policy_api.DTOs;

public record class PolicyDetailsDTO : PolicyBaseDTO
{
    [JsonPropertyName("dataCriacao")]
    [JsonPropertyOrderAttribute(6)]
    public DateTime? DataCriacaoRegistro { get; init; }

    [JsonPropertyName("dataAlteracao")]
    [JsonPropertyOrderAttribute(7)]
    public DateTime? DataAlteracaoRegistro { get; init; }

    [JsonPropertyName("usuarioCriacao")]
    [JsonPropertyOrderAttribute(8)]
    public int? UsuarioCriacaoRegistro { get; init; }

    [JsonPropertyName("usuarioAlteracao")]
    [JsonPropertyOrderAttribute(9)]
    public int? UsuarioAlteracaoRegistro { get; init; }

    [JsonPropertyOrderAttribute(10)]
    public IEnumerable<InstallmentDetailsDTO> Installments { get; init; }

}
