using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Contracts;

public interface IInstallmentRepository
{
    Task<InstallmentEntity> GetByIdAsync(int entityID);
    Task UpdateAsync(InstallmentEntity installmentEntity);
}
