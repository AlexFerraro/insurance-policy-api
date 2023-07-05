using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Contracts;

public interface IInstallmentDomainService
{
    Task UpdateInstallmentiesAsync(IEnumerable<InstallmentEntity> installmentiesToUpdate);
    Task RegisterPaymentAsync(long policyId, DateOnly datePayment);
}
