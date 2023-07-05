using insurance_policy_api.Interfaces;
using insurance_policy_api_domain.Contracts;
using insurance_policy_api_infrastructure.Interfaces;

namespace insurance_policy_api.Services;

public class InstallmentAppService : IInstallmentAppService
{
    private readonly IInstallmentDomainService _installmentDomainService;
    private readonly IUnityOfWork _unityOfWork;

    public InstallmentAppService(IInstallmentDomainService installmentDomainService
                                    , IUnityOfWork unityOfWork) =>
        (_installmentDomainService, _unityOfWork) = (installmentDomainService, unityOfWork);


    public async Task RegisterPaymentForPolicyAsync(long entityId, DateOnly datePayment)
    {
        await _installmentDomainService.RegisterPaymentAsync(entityId, datePayment);

        await _unityOfWork.CommitAsync();
    }
}
