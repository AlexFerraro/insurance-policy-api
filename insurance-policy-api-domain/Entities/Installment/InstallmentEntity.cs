namespace insurance_policy_api_domain.Entities.Installment;

public class InstallmentEntity : EntityBase<int>
{
    public int IdApolice { get; set; }
    public decimal Premio { get; set; }
    public MetodoPagamento FormaPagamento { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime? DataPago { get; set; } = null;
    public decimal? Juros { get; set; } = null;
    public string Situacao { get; set; }
    public PolicyEntity Apolice { get; set; }

    public InstallmentEntity(int installmentID, int idApolice, decimal premio
        , MetodoPagamento formaPagamento, DateTime dataPagamento
        , string situacao, PolicyEntity apolice) : base(installmentID)
    {
        IdApolice = idApolice;
        Premio = premio;
        FormaPagamento = formaPagamento;
        DataPagamento = dataPagamento;
        Situacao = situacao;
        Apolice = apolice;
    }
}
