namespace insurance_policy_api_domain.Entities;

public abstract class EntityBase<T>
{
    public T EntityID { get; private set; }
    public DateOnly RecordCreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? RegistrationChangeDate { get; set; } = null;
    public int UserCreationRecord { get; set; } = 1;
    public int? UserRecordChange { get; set; } = null;

    public EntityBase(T entityID) =>
        EntityID = entityID;
}
