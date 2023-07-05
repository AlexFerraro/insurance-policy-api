using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Exceptions;

namespace insurance_policy_api_domain.Services;

public class PolicyDomainService : IPolicyDomainService
{
    private readonly IPolicyRepository _policyRepository;

    public PolicyDomainService(IPolicyRepository policyRepository) =>
        (_policyRepository) = (policyRepository);

    public async Task CreateNewPolicyAsync(PolicyEntity policyEntity) =>
        await _policyRepository.AddAsync(policyEntity);

    public async Task<PolicyEntity> RetrievePolicyByIdAsync(long policyId) =>
        await _policyRepository.GetByIdAsync(policyId);

    public async Task<IEnumerable<PolicyEntity>> RetrieveAllPoliciesAsync(int skip, int take) =>
        await _policyRepository.GetAllAsync(skip, take);

    public async Task UpdatePolicyAsync(PolicyEntity policyEntityToUpdate)
    {
        var existingPolicy = await _policyRepository.GetByIdAsync(policyEntityToUpdate.EntityID);

        if (existingPolicy is null)
            throw new PolicyNotFoundException($"The policy with ID {policyEntityToUpdate.EntityID} was not found in the database during the policy update request.");

        await _policyRepository.UpdateAsync(policyEntityToUpdate);
    }
}
