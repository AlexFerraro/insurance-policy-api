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
        await _policyDbContext.AddAsync(policyEntity);


    public async Task<PolicyEntity> GetByIdAsync(int entityID, bool asNoTracking = true)
    {
        return asNoTracking
            ? await _policyDbContext.Policies
                                .Include(p => p.Parcelas)
                                .FirstOrDefaultAsync(f => f.EntityID == entityID)
            : await _policyDbContext.Policies.AsTracking()
                                .Include(p => p.Parcelas)
                                .FirstOrDefaultAsync(f => f.EntityID == entityID);
    }


    public async Task<IEnumerable<PolicyEntity>> GetAllAsync(int skip, int take) =>
        await _policyDbContext.Policies.Include(i => i.Parcelas).Skip(skip).Take(take).ToListAsync();

    public async Task UpdateAsync(PolicyEntity policyEntity) =>
        _policyDbContext.Update(policyEntity);

    //Comportamento movido para o UnityOfWork
    //public async Task CommitAsync()
    //{
    //    await _policyDbContext.SaveChangesAsync();
    //}
}
