using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Entities;

public class PolicyEntity : EntityBase<int>
{
    public string? Description { get; set; }
    public string? Cpf { get; set; }
    public string? Situation { get; set; }
    public decimal? TotalPrize { get; set; }

    public ICollection<InstallmentEntity> Installments { get; set; }

    public PolicyEntity(int policyID, string description, string cpf
        , string situation, decimal totalPrize
        , ICollection<InstallmentEntity> installments) : base(policyID)
    {
        Description = description;
        Cpf = cpf;
        Situation = situation;
        TotalPrize = totalPrize;
        Installments = installments;
    }

    //Exclusivo para o Entity Framework
    private PolicyEntity() : base(0)
    {

    }
}
