namespace insurance_policy_api.Interfaces;

public interface IInstallmentAppService
{
    Task RegisterPaymentAsync(int entityId, DateOnly datePayment);
}
