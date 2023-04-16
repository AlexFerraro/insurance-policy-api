using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insurance_policy_api_domain.Excepitions;

public class PaymentAlreadyMadeException : Exception
{
    public PaymentAlreadyMadeException() : base() { }

    public PaymentAlreadyMadeException(string message) : base(message) { }

    public PaymentAlreadyMadeException(string message, Exception innerException) : base(message, innerException) { }
}
