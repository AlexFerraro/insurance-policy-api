using insurance_policy_api_domain.Entities;

namespace insurance_policy_api_domain.Contracts;

public interface IPolicyRepository
{
    Task AddAsync(PolicyEntity policyEntity);
    Task<PolicyEntity> GetByIdAsync(int entityID);
    Task<IEnumerable<PolicyEntity>> GetAllAsync();
    Task UpdateAsync();
    Task CommitAsync();
}
