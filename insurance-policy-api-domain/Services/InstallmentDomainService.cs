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
                throw new InstallmentNotFoundException($"The installment with ID {installment.EntityID} was not found in the database during the request to update the installments of a policy.");
        }

        await _installmentRepository.UpdateRangeAsync(installmentiesToUpdate);
    }

    public async Task RegisterPaymentAsync(int policyId, DateOnly paidDate)
    {
        var installmentPayment = await _installmentRepository.GetByIdAsync(policyId);

        if (installmentPayment is null)
            throw new InstallmentNotFoundException($"The installment with ID {policyId} was not found in the database during the payment registration.");

        if (installmentPayment.IsPaid())
            throw new PaymentAlreadyMadeException($"The installment with ID {policyId} is already paid and it is not possible to make the payment again.");

        UpdateInstallmentPaymentData(installmentPayment, paidDate);

        if (installmentPayment.IsInstallmentOverdue())
        {
            ApplyInterestIfOverdue(installmentPayment, paidDate);
        }

        await _installmentRepository.UpdateAsync(installmentPayment);
    }

    private void UpdateInstallmentPaymentData(InstallmentEntity installmentPayment, DateOnly paidDate)
    {
        installmentPayment.PaidDate = paidDate;
        installmentPayment.Situation = "PAGO";
        installmentPayment.RegistrationChangeDate = DateOnly.FromDateTime(DateTime.Now);
        installmentPayment.UserRecordChange = 3;
    }

    private void ApplyInterestIfOverdue(InstallmentEntity installmentPayment, DateOnly paidDate)
    {
        int daysLate = paidDate.DayNumber - installmentPayment.PaymentDate.Value.DayNumber;
        decimal interestRate = GetInterestRate(installmentPayment.PaymentMethod.Value);
        installmentPayment.Interest = installmentPayment.Premium.Value * interestRate * daysLate;   
    }

    private decimal GetInterestRate(PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.CARTAO => 0.03m,
            PaymentMethod.BOLETO => 0.01m,
            PaymentMethod.DINHEIRO => 0.05m,
            _ => 0m
        };
    }
} 
