using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace insurance_policy_api_infrastructure.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly PolicyDbContext _policyDbContext;

    public PolicyRepository(PolicyDbContext context) =>
        _policyDbContext = context;

    public async Task AddAsync(PolicyEntity policyEntity) =>
        await _policyDbContext.Policies.AddAsync(policyEntity);

    public async Task<PolicyEntity> GetByIdAsync(long entityID) =>
        await _policyDbContext.Policies.Include(i => i.Installments).FirstOrDefaultAsync(f => f.EntityID == entityID);

    public async Task<IEnumerable<PolicyEntity>> GetAllAsync(int skip, int take) =>
        await _policyDbContext.Policies.Include(i => i.Installments).Skip(skip).Take(take).ToListAsync();

    public async Task UpdateAsync(PolicyEntity policyEntity) =>
        await Task.Run(() => _policyDbContext.Policies.Update(policyEntity));

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
