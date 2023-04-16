using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insurance_policy_api_domain.Excepitions;

internal class InstallmentNotFoundException : Exception
{
    public InstallmentNotFoundException() : base() { }

    public InstallmentNotFoundException(string message) : base(message) { }

    public InstallmentNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

