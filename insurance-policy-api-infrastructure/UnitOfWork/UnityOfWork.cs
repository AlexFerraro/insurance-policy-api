using insurance_policy_api_infrastructure.Contexts;
using insurance_policy_api_infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace insurance_policy_api_infrastructure.UnitOfWork;

public class UnityOfWork : IUnityOfWork
{
    private readonly PolicyDbContext _policyDbContext;
    private bool disposedValue;

    public UnityOfWork(PolicyDbContext policyDbContext) => _policyDbContext = policyDbContext;

    public async Task CommitAsync()
    {
        await _policyDbContext.SaveChangesAsync();
    }

    //This is a Sweet Code.
    public async Task RollBack()
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _policyDbContext.Dispose();
        }
    }
}
