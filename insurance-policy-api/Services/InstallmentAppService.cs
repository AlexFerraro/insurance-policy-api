using AutoMapper;
using insurance_policy_api.Interfaces;
using insurance_policy_api_domain.Contracts;
using insurance_policy_api_infrastructure.Interfaces;

namespace insurance_policy_api.Services;

public class InstallmentAppService : IInstallmentAppService
{
    private readonly IInstallmentDomainService _installmentDomainService;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public InstallmentAppService(IInstallmentDomainService installmentDomainService
                                    , IUnityOfWork unityOfWork, IMapper mapper) =>
        (_installmentDomainService, _unityOfWork, _mapper) = (installmentDomainService, unityOfWork, mapper);


    public async Task RegisterPaymentAsync(int entityId, DateOnly datePayment)
    {
        await _installmentDomainService.RegisterPaymentForPolicyAsync(entityId, datePayment);

        await _unityOfWork.CommitAsync();
    }
}
