namespace insurance_policy_api_domain.Entities;

public abstract class EntityBase<T>
{
    public T EntityID { get; private set; }
    public DateTime DataCriacaoRegistro { get; set; }
    public DateTime DataAlteracaoRegistro { get; set; }
    public int UsuarioCriacaoRegistro { get; set; }
    public int UsuarioAlteracaoRegistro { get; set; }

    public EntityBase(T entityID) =>
        EntityID = entityID;
}
