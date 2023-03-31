using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Entities.Installment;
using Microsoft.EntityFrameworkCore;

namespace insurance_policy_api_infrastructure.Contexts;

public class PolicyDbContext : DbContext
{
    public DbSet<PolicyEntity> Policies { get; set; }
    public DbSet<InstallmentEntity> Installments { get; set; }

    public PolicyDbContext(DbContextOptions<PolicyDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder/*.UseLazyProxies()*/
            .UseNpgsql("Server=localhost,5432;Database=apolicy_db;User Id=postgres;Password=postgres;TrustServerCertificate=True", providerOptions => providerOptions.EnableRetryOnFailure().CommandTimeout(5));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PolicyEntity>(policy =>
        {
            policy.ToTable("apolice");
            policy.HasKey(p => p.EntityID).HasName("pk_apolice_id");
            policy.Property(p => p.EntityID).HasColumnName("id").ValueGeneratedNever().IsRequired();
            policy.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(50).IsRequired();
            policy.Property(p => p.Cpf).HasColumnName("cpf").IsRequired();
            policy.Property(p => p.Situacao).HasColumnName("situacao").HasMaxLength(10).IsRequired();
            policy.Property(p => p.PremioTotal).HasColumnName("premio_total").HasColumnType("decimal(10, 2)").IsRequired();
            policy.Property(p => p.DataCriacaoRegistro).HasColumnName("data_criacao_registro").IsRequired();
            policy.Property(p => p.DataAlteracaoRegistro).HasColumnName("data_alteracao_registro").IsRequired();
            policy.Property(p => p.UsuarioCriacaoRegistro).HasColumnName("usuario_criacao_registro").IsRequired();
            policy.Property(p => p.UsuarioAlteracaoRegistro).HasColumnName("usuario_alteracao_registro").IsRequired();

            policy.HasMany(p => p.Parcelas)
                  .WithOne(p => p.Apolice)
                  .HasForeignKey(p => p.IdApolice)
                  .HasConstraintName("fk_apo_par")
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
        });

        modelBuilder.Entity<InstallmentEntity>(installment =>
        {
            installment.ToTable("parcela");
            installment.HasKey(p => p.EntityID).HasName("pk_parcela_id");
            installment.Property(p => p.EntityID).HasColumnName("id").ValueGeneratedNever().IsRequired();
            installment.Property(p => p.IdApolice).HasColumnName("id_apolice").IsRequired();
            installment.Property(p => p.Premio).HasColumnName("premio").HasColumnType("decimal(10, 2)").IsRequired();
            installment.Property(p => p.FormaPagamento).HasColumnName("forma_pagamento").HasMaxLength(50).IsRequired();
            installment.Property(p => p.DataPagamento).HasColumnName("data_pagamento").IsRequired();
            installment.Property(p => p.DataPago).HasColumnName("data_pago");
            installment.Property(p => p.Juros).HasColumnName("juros").HasColumnType("decimal(10, 2)");
            installment.Property(p => p.Situacao).HasColumnName("situacao").HasMaxLength(10).IsRequired();
            installment.Property(p => p.DataCriacaoRegistro).HasColumnName("data_criacao_registro").IsRequired();
            installment.Property(p => p.DataAlteracaoRegistro).HasColumnName("data_alteracao_registro").IsRequired();
            installment.Property(p => p.UsuarioCriacaoRegistro).HasColumnName("usuario_criacao_registro").IsRequired();
            installment.Property(p => p.UsuarioAlteracaoRegistro).HasColumnName("usuario_alteracao_registro").IsRequired();
        });
    }
}
