using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities.Installment;
using insurance_policy_api_domain.Excepitions;

namespace insurance_policy_api_domain.Services;

public class InstallmentDomainService : IInstallmentDomainService
{
    private readonly IInstallmentRepository _installmentRepository;

    public InstallmentDomainService(IInstallmentRepository installmentRepository) =>
        _installmentRepository = installmentRepository;

    public async Task UpdateInstallmentiesAsync(IEnumerable<InstallmentEntity> installmentiesToUpdate)
    {
        foreach (var installment in installmentiesToUpdate)
        {
            var existingInstallment = await _installmentRepository.GetByIdAsync(installment.EntityID);

            if (existingInstallment is null)
                throw new InstallmentNotFoundException($"A parcela com Id {installment.EntityID} não foi encontrada no banco de dados.");
        }

        await _installmentRepository.UpdateRangeAsync(installmentiesToUpdate);
    }

    public async Task RegisterPaymentForPolicyAsync(int policyId, DateOnly paidDate)
    {
        var installmentPayment = await _installmentRepository.GetByIdAsync(policyId);

        if (installmentPayment is null)
            throw new InstallmentNotFoundException("A parcela informada não foi encontrada.");

        if (installmentPayment.IsPaid())
            throw new PaymentAlreadyMadeException("A parcela informada já se encontra paga.");

        installmentPayment.PaidDate = paidDate;
        installmentPayment.Situation = "PAGO";
        installmentPayment.RegistrationChangeDate = DateOnly.FromDateTime(DateTime.Now);
        installmentPayment.UserRecordChange = 3;

        if (installmentPayment.IsInstallmentOverdue())
        {
            int daysLate = paidDate.DayNumber - installmentPayment.PaymentDate.Value.DayNumber;

            decimal interestRate = installmentPayment.PaymentMethod.Value switch
            {
                PaymentMethod.CARTAO => 0.03m,
                PaymentMethod.BOLETO => 0.01m,
                PaymentMethod.DINHEIRO => 0.05m,
                _ => 0m
            };

            installmentPayment.Interest = installmentPayment.Premium.Value * interestRate * daysLate;
        }

        await _installmentRepository.UpdateAsync(installmentPayment);
    }
}
