namespace insurance_policy_api_domain.Entities.Installment;

public class InstallmentEntity : EntityBase<int>
{
    public int? IdApolice { get; set; }
    public decimal? Premium { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? PaidDate { get; set; } = null;
    public decimal? Fees { get; set; } = null;
    public string? Situation { get; set; } = null;
    public PolicyEntity? Policy { get; set; }

    public InstallmentEntity(int installmentID, int idApolice, decimal premium
        , PaymentMethod paymentMethod, DateTime paymentDate
        , string situation, PolicyEntity policy) : base(installmentID)
    {
        Premium = premium;
        PaymentMethod = paymentMethod;
        PaymentDate = paymentDate;
        Situation = situation;
        Policy = policy;
    }

    //Exclusivo para o Entity Framework
    private InstallmentEntity() : base(0)
    {

    }
}
