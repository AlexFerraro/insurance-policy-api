using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Contracts;

public interface IInstallmentRepository : IDisposable
{
    Task<InstallmentEntity> GetByIdAsync(int entityID);
    Task UpdateAsync(InstallmentEntity installmentEntity);
    Task UpdateRangeAsync(IEnumerable<InstallmentEntity> installmentiesEntity);
}
