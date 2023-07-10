using insurance_policy_api_application.DTOs;

namespace insurance_policy_api_application.Interfaces;

public interface IPolicyAppService
{
    Task<PolicyDTO> CreatePolicyAsync(PolicyDTO policyDto);
    Task<PolicyDTO> GetPolicyByIdAsync(long entityId);
    Task<IEnumerable<PolicyDetailsDTO>> GetAllPoliciesAsync(int skip, int take);
    Task<PolicyDTO> UpdatePolicyAsync(PolicyDTO policyDto);
}