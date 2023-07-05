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
        modelBuilder.Entity<InstallmentEntity>(entity =>
        {
            entity.HasKey(e => e.EntityID).HasName("pk_installment");

            entity.ToTable("installment");

            entity.Property(e => e.EntityID)
                .UseIdentityAlwaysColumn()
                .HasColumnName("installment_id");
            entity.Property(e => e.RecordCreationDate).HasColumnName("creation_date");
            entity.Property(e => e.UserCreationRecord).HasColumnName("creation_user");
            entity.Property(e => e.Interest)
                .HasPrecision(10, 2)
                .HasColumnName("interest");
            entity.Property(e => e.RegistrationChangeDate).HasColumnName("modification_date");
            entity.Property(e => e.UserRecordChange).HasColumnName("modification_user");
            entity.Property(e => e.PaidDate).HasColumnName("paid_date");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(10)
                .HasColumnName("payment_method");
            entity.Property(e => e.PolicyFK).HasColumnName("policy_fk");
            entity.Property(e => e.Premium)
                .HasPrecision(10, 2)
                .HasColumnName("premium");
            entity.Property(e => e.Situation)
                .HasMaxLength(10)
                .HasColumnName("status");

            entity.HasOne(d => d.Policy).WithMany(p => p.Installments)
                .HasForeignKey(d => d.PolicyFK)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_policy_installment");
        });

        modelBuilder.Entity<PolicyEntity>(entity =>
        {
            entity.HasKey(e => e.EntityID).HasName("pk_policy");

            entity.ToTable("policy");

            entity.Property(e => e.EntityID)
                .UseIdentityAlwaysColumn()
                .HasColumnName("policy_id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(12)
                .HasColumnName("cpf");
            entity.Property(e => e.RecordCreationDate).HasColumnName("creation_date");
            entity.Property(e => e.UserCreationRecord).HasColumnName("creation_user");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.RegistrationChangeDate).HasColumnName("modification_date");
            entity.Property(e => e.UserRecordChange).HasColumnName("modification_user");
            entity.Property(e => e.Situation)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.TotalPremium)
                .HasPrecision(10, 2)
                .HasColumnName("total_premium");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}