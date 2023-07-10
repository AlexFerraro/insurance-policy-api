using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities.Installment;
using insurance_policy_api_domain.Exceptions;
using insurance_policy_api_domain.Services;
using Moq;
using NUnit.Framework;

namespace insurance_policy_api_test.Domain.Services;

[TestFixture]
public class InstallmentDomainServiceTests
{
    private Mock<IInstallmentRepository> _mockInstallmentRepository;
    private InstallmentDomainService _installmentDomainService;

    [SetUp]
    public void Setup()
    {
        _mockInstallmentRepository = new Mock<IInstallmentRepository>();
        _installmentDomainService = new InstallmentDomainService(_mockInstallmentRepository.Object);
    }

    [Test]
    public void UpdateInstallmentiesAsync_ExistingInstallments_NoExceptionThrown()
    {
        // Arrange
        var installment1 = new InstallmentEntity(1, 1, 100, PaymentMethod.CARTAO, new DateOnly(2023, 7, 10), null, null);
        var installment2 = new InstallmentEntity(2, 1, 150, PaymentMethod.BOLETO, new DateOnly(2023, 7, 11), null, null);
        var installmentiesToUpdate = new List<InstallmentEntity> { installment1, installment2 };

        _mockInstallmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(installment1);

        // Act
        Func<Task> updateAction = async () => await _installmentDomainService.UpdateInstallmentiesAsync(installmentiesToUpdate);

        // Assert
        Assert.That(updateAction, Throws.Nothing);
    }

    [Test]
    public void UpdateInstallmentiesAsync_NonExistingInstallment_ThrowsInstallmentNotFoundException()
    {
        // Arrange
        var installment = new InstallmentEntity(1, 1, 100, PaymentMethod.CARTAO, new DateOnly(2023, 7, 10), null, null);
        var installmentiesToUpdate = new List<InstallmentEntity> { installment };

        _mockInstallmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long installmentId) => null);

        // Act
        Func<Task> updateAction = async () => await _installmentDomainService.UpdateInstallmentiesAsync(installmentiesToUpdate);

        // Assert
        Assert.That(updateAction, Throws.TypeOf<InstallmentNotFoundException>()
            .With.Message.EqualTo($"The installment with ID {installment.EntityID} was not found in the database during the request to update the installments of a policy."));
    }

    [Test]
    public async Task RegisterPaymentAsync_ExistingInstallment_NotPaid_PaymentRegistered()
    {
        // Arrange
        var installmentId = 1;
        var paymentDate = new DateOnly(2023, 7, 10);
        var installmentPayment = new InstallmentEntity(installmentId, 1, 100, PaymentMethod.CARTAO, new DateOnly(2023, 7, 9), null, null);

        _mockInstallmentRepository.Setup(r => r.GetByIdAsync(installmentId))
            .ReturnsAsync(installmentPayment);

        // Act
        await _installmentDomainService.RegisterPaymentAsync(installmentId, paymentDate);

        // Assert
        Assert.AreEqual(paymentDate, installmentPayment.PaidDate);
        Assert.AreEqual("PAGO", installmentPayment.Situation);
        Assert.AreEqual(DateOnly.FromDateTime(DateTime.UtcNow), installmentPayment.RegistrationChangeDate);
        Assert.AreEqual(3, installmentPayment.UserRecordChange);

        // Verifies
        _mockInstallmentRepository.Verify(r => r.UpdateAsync(installmentPayment), Times.Once);
    }

    [Test]
    public void RegisterPaymentAsync_ExistingInstallment_Paid_ThrowsPaymentAlreadyMadeException()
    {
        // Arrange
        var installmentId = 1;
        var paymentDate = new DateOnly(2023, 7, 10);
        var installmentPayment = new InstallmentEntity(installmentId, 1, 100, PaymentMethod.CARTAO, new DateOnly(2023, 7, 9), "PAGO", null);

        _mockInstallmentRepository.Setup(r => r.GetByIdAsync(installmentId))
            .ReturnsAsync(installmentPayment);

        // Act
        Func<Task> registerPaymentAction = async () =>
            await _installmentDomainService.RegisterPaymentAsync(installmentId, paymentDate);

        // Assert
        Assert.That(registerPaymentAction, Throws.TypeOf<PaymentAlreadyMadeException>()
            .With.Message.EqualTo($"The installment with ID {installmentId} is already paid and it is not possible to make the payment again."));
        
        // Verifies
        _mockInstallmentRepository.Verify(r => r.UpdateAsync(It.IsAny<InstallmentEntity>()), Times.Never);
    }

    [Test]
    public void RegisterPaymentAsync_NonExistingInstallment_ThrowsInstallmentNotFoundException()
    {
        // Arrange
        var installmentId = 1;
        var paymentDate = new DateOnly(2023, 7, 10);

        _mockInstallmentRepository.Setup(r => r.GetByIdAsync(installmentId))
            .ReturnsAsync((InstallmentEntity)null);

        // Act
        Func<Task> registerPaymentAction = async () =>
            await _installmentDomainService.RegisterPaymentAsync(installmentId, paymentDate);

        // Assert
        Assert.That(registerPaymentAction, Throws.TypeOf<InstallmentNotFoundException>()
            .With.Message.EqualTo($"The installment with ID {installmentId} was not found in the database during the payment registration."));

        // Verifies
        _mockInstallmentRepository.Verify(r => r.UpdateAsync(It.IsAny<InstallmentEntity>()), Times.Never);
    }
}