using insurance_policy_api_domain.Entities;

namespace insurance_policy_api_domain.Contracts;

public interface IPolicyDomainService
{
    Task CreateNewPolicyAsync(PolicyEntity policyEntity);
    Task<PolicyEntity> RetrievePolicyByIdAsync(int policyId);
    Task<IEnumerable<PolicyEntity>> RetrieveAllPoliciesAsync(int skip, int take);
    Task UpdatePolicyAsync(PolicyEntity policyEntity);
    Task RegisterPaymentForPolicyAsync(int InstallmentId, DateTime datePagamento);
}
