using insurance_policy_api_domain.Entities;

namespace insurance_policy_api_domain.Contracts;

public interface IPolicyRepository : IDisposable
{
    Task AddAsync(PolicyEntity policyEntity);
    Task<PolicyEntity> GetByIdAsync(long entityID);
    Task<IEnumerable<PolicyEntity>> GetAllAsync(int skip, int take);
    Task UpdateAsync(PolicyEntity policyEntity);

    //Assinatura movida para o IUnityOfWork
    //Task CommitAsync();
}
