namespace insurance_policy_api.Interfaces;

public interface IInstallmentAppService
{
    Task RegisterPaymentForPolicyAsync(long entityId, DateOnly datePayment);
}
