namespace insurance_policy_api_domain.Entities.Installment;

public class InstallmentEntity
{
    public int Id { get; set; }
    public int IdApolice { get; set; }
    public decimal Premio { get; set; }
    public MetodoPagamento FormaPagamento { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime? DataPago { get; set; }
    public decimal? Juros { get; set; }
    public string Situacao { get; set; }
    public PolicyEntity Apolice { get; set; }
}
