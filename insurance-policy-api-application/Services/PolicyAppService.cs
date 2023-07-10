using AutoMapper;
using insurance_policy_api_application.DTOs;
using insurance_policy_api_application.Interfaces;
using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_infrastructure.Interfaces;

namespace insurance_policy_api_application.Services;

public class PolicyAppService : IPolicyAppService
{
    private readonly IPolicyDomainService _policyDomainService;
    private readonly IInstallmentDomainService _installmentDomainService;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public PolicyAppService(IPolicyDomainService policyDomainService, IInstallmentDomainService installmentDomainService
                                , IUnityOfWork unityOfWork, IMapper mapper)
        => (_policyDomainService, _installmentDomainService, _unityOfWork, _mapper)
                = (policyDomainService, installmentDomainService, unityOfWork, mapper);


    public async Task<PolicyDTO> CreatePolicyAsync(PolicyDTO policyDto)
    {
        var policyEntity = _mapper.Map<PolicyEntity>(policyDto);

        await _policyDomainService.CreateNewPolicyAsync(policyEntity);

        await _unityOfWork.CommitAsync();

        return _mapper.Map<PolicyDTO>(policyEntity);
    }

    public async Task<PolicyDTO> GetPolicyByIdAsync(long entityId) =>
        _mapper.Map<PolicyDTO>(await _policyDomainService.RetrievePolicyByIdAsync(entityId));

    public async Task<IEnumerable<PolicyDetailsDTO>> GetAllPoliciesAsync(int skip, int take)
    {
        var policies = await _policyDomainService.RetrieveAllPoliciesAsync(skip, take);

        return _mapper.Map<IEnumerable<PolicyDetailsDTO>>(policies);
    }

    public async Task<PolicyDTO> UpdatePolicyAsync(PolicyDTO policyDto)
    {
        var policyEntity = _mapper.Map<PolicyEntity>(policyDto);

        await _policyDomainService.UpdatePolicyAsync(policyEntity);
        await UpdatePolicyInstallmentiesAsync(policyEntity);

        await _unityOfWork.CommitAsync();

        return _mapper.Map<PolicyDTO>(policyEntity);
    }

    private async Task UpdatePolicyInstallmentiesAsync(PolicyEntity policyEntity)
    {
        if (policyEntity.Installments is not null && policyEntity.Installments.Count > 0)
            await _installmentDomainService.UpdateInstallmentiesAsync(policyEntity.Installments);
    }
}
