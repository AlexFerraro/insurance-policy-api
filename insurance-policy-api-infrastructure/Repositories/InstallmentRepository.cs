using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities.Installment;
using insurance_policy_api_infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace insurance_policy_api_infrastructure.Repositories;

public class InstallmentRepository : IInstallmentRepository
{
    private readonly PolicyDbContext _policyDbContext;

    public InstallmentRepository(PolicyDbContext context) =>
        _policyDbContext = context;

    public async Task<InstallmentEntity> GetByIdAsync(long entityID) =>
        await _policyDbContext.Installments
                    .FirstOrDefaultAsync(installment => installment.EntityID == entityID);

    public async Task UpdateAsync(InstallmentEntity installmentEntity) =>
        await Task.Run(() => _policyDbContext.Installments.Update(installmentEntity));

    public async Task UpdateRangeAsync(IEnumerable<InstallmentEntity> installmentiesEntity) =>
        await Task.Run(() => _policyDbContext.Installments.UpdateRange(installmentiesEntity));

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
