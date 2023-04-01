using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insurance_policy_api_infrastructure.Interfaces;

public interface IUnityOfWork
{
    Task CommitAsync();
    Task RollBack();
}
