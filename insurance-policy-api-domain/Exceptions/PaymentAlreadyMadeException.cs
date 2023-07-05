namespace insurance_policy_api_domain.Exceptions;

public class PaymentAlreadyMadeException : Exception
{
    public PaymentAlreadyMadeException() : base() { }

    public PaymentAlreadyMadeException(string message) : base(message) { }

    public PaymentAlreadyMadeException(string message, Exception innerException) : base(message, innerException) { }
}
