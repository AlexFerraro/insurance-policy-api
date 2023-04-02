using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Entities.Installment;
using insurance_policy_api_domain.Excepitions;
using insurance_policy_api_domain.Helpers;

namespace insurance_policy_api_domain.Services;

public class PolicyDomainService : IPolicyDomainService
{
    private readonly IPolicyRepository _policyRepository;
    private readonly IInstallmentRepository _installmentRepository;

    public PolicyDomainService(IPolicyRepository policyRepository, IInstallmentRepository installmentRepository) =>
        (_policyRepository, _installmentRepository) = (policyRepository, installmentRepository);

    public async Task CreateNewPolicyAsync(PolicyEntity policyEntity) =>
        await _policyRepository.AddAsync(policyEntity);

    public async Task<PolicyEntity> RetrievePolicyByIdAsync(int policyId) =>
        await _policyRepository.GetByIdAsync(policyId);

    public async Task<IEnumerable<PolicyEntity>> RetrieveAllPoliciesAsync(int skip, int take) =>
        await _policyRepository.GetAllAsync(skip, take);

    public async Task UpdatePolicyAsync(PolicyEntity policyEntity)
    {
        var policyToUpdate = await _policyRepository.GetByIdAsync(policyEntity.EntityID, false);

        if (policyToUpdate is null)
            throw new NotFoundException($"Apólice não encontrada no banco de dados.");

        policyToUpdate.Description = policyEntity.Description;
        policyToUpdate.Cpf = policyEntity.Cpf;
        policyToUpdate.Situation = policyEntity.Situation;
        policyToUpdate.TotalPrize = policyEntity.TotalPrize;
        policyToUpdate.RegistrationChangeDate = DateOnly.FromDateTime(DateTime.Now);
        policyToUpdate.UserRecordChange = 2;

        if (!policyEntity.Installments.IsNullOrEmpty()) 
        {
            try
            {
                policyEntity.Installments
                    .ForEach(installmentNew =>
                    {
                        var installmentToUpdate = policyToUpdate.Installments
                                        .First(f => installmentNew.EntityID == f.EntityID );

                        installmentToUpdate.Premium = installmentNew.Premium;
                        installmentToUpdate.PaymentMethod = installmentNew.PaymentMethod;
                        installmentToUpdate.PaymentDate = installmentNew.PaymentDate;
                        installmentToUpdate.RegistrationChangeDate = DateOnly.FromDateTime(DateTime.Now);
                        installmentToUpdate.UserRecordChange = 2;
                    });
            }
            catch (InvalidOperationException ex)
            {
                throw new NotFoundException($"Uma das parcelas não foi encontrada na base de dados.", ex);
            }
        }

        await _policyRepository.UpdateAsync(policyToUpdate);
    }

    public async Task RegisterPaymentForPolicyAsync(int policyId, DateOnly paidDate)
    {
        var installmentPayment = await _installmentRepository.GetByIdAsync(policyId);

        if (installmentPayment is null)
            throw new NotFoundException($"Parcela não encontrada no banco de dados.");

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

            installmentPayment.Fees = installmentPayment.Premium.Value * interestRate * daysLate;
        }

        await _installmentRepository.UpdateAsync(installmentPayment);
    }
}
