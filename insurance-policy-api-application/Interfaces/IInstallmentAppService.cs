namespace insurance_policy_api_application.Interfaces;

public interface IInstallmentAppService
{
    Task RegisterPaymentForPolicyAsync(long entityId, DateOnly datePayment);
}
