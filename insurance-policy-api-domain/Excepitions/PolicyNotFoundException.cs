namespace insurance_policy_api_domain.Excepitions;

public class PolicyNotFoundException : Exception
{
    public PolicyNotFoundException() : base() { }

    public PolicyNotFoundException(string message) : base(message) { }

    public PolicyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
