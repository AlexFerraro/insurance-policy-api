using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Contracts;

public interface IInstallmentDomainService
{
    Task UpdateInstallmentiesAsync(IEnumerable<InstallmentEntity> installmentiesToUpdate);
    Task RegisterPaymentForPolicyAsync(int policyId, DateOnly datePayment);
}
