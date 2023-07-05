using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Entities;

public class PolicyEntity : EntityBase<long>
{
    public string? Description { get; set; } = null;
    public string Cpf { get; set; }
    public string Situation { get; set; }
    public decimal TotalPremium { get; set; }

    public virtual ICollection<InstallmentEntity> Installments { get; set; }

    public PolicyEntity(long policyID, string description, string cpf
        , string situation, decimal totalPrize
        , ICollection<InstallmentEntity> installments) : base(policyID)
    {
        Description = description;
        Cpf = cpf;
        Situation = situation;
        TotalPremium = totalPrize;
        Installments = installments;
    }

    //Exclusivo para o Entity Framework
    private PolicyEntity() : base(0)
    {

    }
}
