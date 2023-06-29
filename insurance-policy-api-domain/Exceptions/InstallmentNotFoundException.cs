namespace insurance_policy_api_domain.Exceptions;

public class InstallmentNotFoundException : Exception
{
    public InstallmentNotFoundException() : base() { }

    public InstallmentNotFoundException(string message) : base(message) { }

    public InstallmentNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

