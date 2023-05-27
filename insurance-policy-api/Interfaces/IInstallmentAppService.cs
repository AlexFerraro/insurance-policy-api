namespace insurance_policy_api.Interfaces;

public interface IInstallmentAppService
{
    Task RegisterPaymentForPolicyAsync(int entityId, DateOnly datePayment);
}
