namespace insurance_policy_api_domain.Contracts;

public interface IInstallmentDomainService
{
    Task RegisterPaymentForPolicyAsync(int policyId, DateOnly datePayment);
}
