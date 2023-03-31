using insurance_policy_api_domain.Entities;

namespace insurance_policy_api_domain.Contracts;

public interface IPolicyDomainService
{
    Task CreatePolicyAsync(PolicyEntity policyEntity);
    Task<PolicyEntity> GetPolicyByIdAsync(int policyId);
    Task<IEnumerable<PolicyEntity>> GetAllPoliciesAsync();
    Task UpdatePolicyAsync();
    Task RegisterPaymentAsync(int InstallmentId, DateTime datePagamento);
}
