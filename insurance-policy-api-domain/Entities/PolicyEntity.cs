﻿using insurance_policy_api_domain.Entities.Installment;

namespace insurance_policy_api_domain.Entities;

public class PolicyEntity : EntityBase<int>
{
    public string Descricao { get; set; }
    public int Cpf { get; set; }
    public string Situacao { get; set; }
    public decimal PremioTotal { get; set; }
    
    public ICollection<InstallmentEntity> Parcelas { get; set; }

    public PolicyEntity(int policyID, string descricao, int cpf
        , string situacao, decimal premioTotal
        , ICollection<InstallmentEntity> parcelas) : base(policyID)
    {
        Descricao = descricao;
        Cpf = cpf;
        Situacao = situacao;
        PremioTotal = premioTotal;
        Parcelas = parcelas;
    }
}