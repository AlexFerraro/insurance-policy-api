namespace insurance_policy_api_domain.Entities;

public abstract class EntityBase<T>
{
    public T EntityID { get; private set; }
    public DateOnly? DataCriacaoRegistro { get; set; } = null;
    public DateOnly? DataAlteracaoRegistro { get; set; } = null;
    public int? UsuarioCriacaoRegistro { get; set; } = null;
    public int? UsuarioAlteracaoRegistro { get; set; } = null;

    public EntityBase(T entityID) =>
        EntityID = entityID;
}
