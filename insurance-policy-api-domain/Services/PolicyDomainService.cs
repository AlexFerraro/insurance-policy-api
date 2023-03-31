using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities;

namespace insurance_policy_api_domain.Services;

public class PolicyDomainService : IPolicyDomainService
{
    private readonly IPolicyRepository _policyRepository;

    public PolicyDomainService(IPolicyRepository policyRepository) => 
        _policyRepository = policyRepository;

    public async Task CreatePolicyAsync(PolicyEntity policyEntity)
    {
        await _policyRepository.AddAsync(policyEntity);
        await _policyRepository.CommitAsync();
    }

    public async Task<PolicyEntity> GetPolicyByIdAsync(int policyId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PolicyEntity>> GetAllPoliciesAsync() =>
        await _policyRepository.GetAllAsync();

    public async Task UpdatePolicyAsync()
    {
        throw new NotImplementedException();
    }

    public async Task RegisterPaymentAsync(int InstallmentId, DateTime datePagamento)
    {
        throw new NotImplementedException();
    }

}
