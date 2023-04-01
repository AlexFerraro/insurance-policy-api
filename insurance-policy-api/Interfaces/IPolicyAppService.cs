using insurance_policy_api.DTOs;
using insurance_policy_api_domain.Entities;

namespace insurance_policy_api.Interfaces;

public interface IPolicyAppService
{
    Task<PolicyDTO> CreatePolicyAsync(PolicyDTO policyDto);
    Task<PolicyDTO> GetPolicyByIdAsync(int entityId);
    Task<IEnumerable<PolicyEntity>> GetAllPoliciesAsync(int skip, int take);
    Task UpdatePolicyAsync();
    Task<PolicyDTO> RegisterPaymentAsync(int entityId, DateTime datePagamento);
}
