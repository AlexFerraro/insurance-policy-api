using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Entities.Installment;
using Microsoft.EntityFrameworkCore;

namespace insurance_policy_api_infrastructure.Contexts;
public partial class PolicyDbContext : DbContext
{
    public PolicyDbContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public PolicyDbContext(DbContextOptions<PolicyDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public virtual DbSet<PolicyEntity> Policies { get; set; }

    public virtual DbSet<InstallmentEntity> Installments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PolicyEntity>(entity =>
        {
            entity.HasKey(e => e.EntityID).HasName("apolice_pkey");

            entity.ToTable("apolice");

            entity.Property(e => e.EntityID)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(12)
                .HasColumnName("cpf");
            entity.Property(e => e.RegistrationChangeDate).HasColumnName("data_alteracao_registro");
            entity.Property(e => e.RecordCreationDate).HasColumnName("data_criacao_registro");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.TotalPrize)
                .HasPrecision(10, 2)
                .HasColumnName("premio_total");
            entity.Property(e => e.Situation)
                .HasMaxLength(10)
                .HasColumnName("situacao");
            entity.Property(e => e.UserRecordChange).HasColumnName("usuario_alteracao_registro");
            entity.Property(e => e.UserCreationRecord).HasColumnName("usuario_criacao_registro");
        });

        modelBuilder.Entity<InstallmentEntity>(entity =>
        {
            entity.HasKey(e => e.EntityID).HasName("parcela_pkey");

            entity.ToTable("parcela");

            entity.Property(e => e.EntityID)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.RegistrationChangeDate).HasColumnName("data_alteracao_registro");
            entity.Property(e => e.RecordCreationDate).HasColumnName("data_criacao_registro");
            entity.Property(e => e.PaymentDate).HasColumnName("data_pagamento");
            entity.Property(e => e.PaidDate).HasColumnName("data_pago");
            entity.Property(e => e.PaymentMethod)
            .HasConversion(
                    v => v.ToString(),
                    v => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), v))
                .HasMaxLength(50)
                .HasColumnName("forma_pagamento");
            entity.Property(e => e.IdApolice).HasColumnName("id_apolice");
            entity.Property(e => e.Interest)
                .HasPrecision(10, 2)
                .HasColumnName("juros");
            entity.Property(e => e.Premium)
                .HasPrecision(10, 2)
                .HasColumnName("premio");
            entity.Property(e => e.Situation)
                .HasMaxLength(10)
                .HasColumnName("situacao");
            entity.Property(e => e.UserRecordChange).HasColumnName("usuario_alteracao_registro");
            entity.Property(e => e.UserCreationRecord).HasColumnName("usuario_criacao_registro");

            entity.HasOne(d => d.Policy).WithMany(p => p.Installments)
                .HasForeignKey(d => d.IdApolice)
                .HasConstraintName("apolice_id_apolice__fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
