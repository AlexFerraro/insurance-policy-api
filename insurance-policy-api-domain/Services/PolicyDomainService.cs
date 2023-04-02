using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Excepitions;
using insurance_policy_api_domain.Helpers;

namespace insurance_policy_api_domain.Services;

public class PolicyDomainService : IPolicyDomainService
{
    private readonly IPolicyRepository _policyRepository;

    public PolicyDomainService(IPolicyRepository policyRepository) =>
        _policyRepository = policyRepository;

    public async Task CreateNewPolicyAsync(PolicyEntity policyEntity) =>
        await _policyRepository.AddAsync(policyEntity);

    public async Task<PolicyEntity> RetrievePolicyByIdAsync(int policyId) =>
        await _policyRepository.GetByIdAsync(policyId);

    public async Task<IEnumerable<PolicyEntity>> RetrieveAllPoliciesAsync(int skip, int take) =>
        await _policyRepository.GetAllAsync(skip, take);

    /*
      Pode ser quebrado em dois métodos para mantero SRP, um para atualizar a apólice e outro para atualizar 
      as parcelas sendo o repositório responsável por alterar as parcelas o installmentRepository.
    */
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

    public async Task RegisterPaymentForPolicyAsync(int policyId, DateTime datePagamento)
    {
        throw new NotImplementedException();
    }
}
