namespace insurance_policy_api.DTOs;

public record class ResponseDTO<T>
{
    public T Data { get; init; }
    public LinkDTO[]? Links { get; init; }
}
