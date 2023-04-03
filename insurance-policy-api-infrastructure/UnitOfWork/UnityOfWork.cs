using insurance_policy_api_infrastructure.Contexts;
using insurance_policy_api_infrastructure.Interfaces;

namespace insurance_policy_api_infrastructure.UnitOfWork;

public class UnityOfWork : IUnityOfWork
{
    private readonly PolicyDbContext _policyDbContext;

    public UnityOfWork(PolicyDbContext policyDbContext) => _policyDbContext = policyDbContext;

    public async Task CommitAsync()
    {
        await _policyDbContext.SaveChangesAsync();
    }

    //This is a Sweet Code.
    public async Task RollBack()
    {
    }
}
