using insurance_policy_api.DTOs;

namespace insurance_policy_api.Interfaces;

public interface IPolicyAppService
{
    Task<PolicyDTO> CreatePolicyAsync(PolicyDTO policyDto);
    Task<PolicyDTO> GetPolicyByIdAsync(long entityId);
    Task<IEnumerable<PolicyDetailsDTO>> GetAllPoliciesAsync(int skip, int take);
    Task<PolicyDTO> UpdatePolicyAsync(PolicyDTO policyDto);
}
