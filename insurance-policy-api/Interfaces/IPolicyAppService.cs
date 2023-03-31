using insurance_policy_api_domain.Entities;

namespace insurance_policy_api.Interfaces;

public interface IPolicyAppService
{
    Task CreatePolicyAsync(PolicyEntity policyEntity);
    Task<PolicyEntity> GetPolicyByIdAsync(int entityID);
    Task<IEnumerable<PolicyEntity>> GetAllPoliciesAsync();
    Task UpdatePolicyAsync();
    Task RegisterPaymentAsync(int entityID, DateTime datePagamento);
}
