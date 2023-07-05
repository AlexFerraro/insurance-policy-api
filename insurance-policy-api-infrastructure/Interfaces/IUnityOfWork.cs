namespace insurance_policy_api_infrastructure.Interfaces;

public interface IUnityOfWork : IDisposable
{
    Task CommitAsync();
    Task RollBack();
}
