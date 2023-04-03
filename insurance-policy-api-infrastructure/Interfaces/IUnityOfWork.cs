namespace insurance_policy_api_infrastructure.Interfaces;

public interface IUnityOfWork
{
    Task CommitAsync();
    Task RollBack();
}
