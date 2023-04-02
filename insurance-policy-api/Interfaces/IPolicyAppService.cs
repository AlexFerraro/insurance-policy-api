using insurance_policy_api.DTOs;

namespace insurance_policy_api.Interfaces;

public interface IPolicyAppService
{
    Task<PolicyDTO> CreatePolicyAsync(PolicyDTO policyDto);
    Task<PolicyDTO> GetPolicyByIdAsync(int entityId);
    Task<IEnumerable<PolicyDetailsDTO>> GetAllPoliciesAsync(int skip, int take);
    Task<PolicyDTO> UpdatePolicyAsync(PolicyDTO policyDto);
    Task<PolicyDTO> RegisterPaymentAsync(int entityId, DateTime datePagamento);
}
