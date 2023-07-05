//using insurance_policy_api_domain.Contracts;
//using insurance_policy_api_domain.Entities;
//using insurance_policy_api_domain.Entities.Installment;
//using insurance_policy_api_domain.Services;
//using Moq;
//using NUnit.Framework;
//using System.Collections.ObjectModel;

//namespace insurance_policy_api_test;

//public class PolicyDomainServiceTests
//{
//    private Mock<IPolicyRepository> _policyRepositoryMock;
//    private Mock<IInstallmentRepository> _installmentRepositoryMock;
//    private PolicyDomainService _policyDomainService;

//    [SetUp]
//    public void Setup()
//    {
//        _policyRepositoryMock = new Mock<IPolicyRepository>();
//        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
//        _policyDomainService = new PolicyDomainService(
//            _policyRepositoryMock.Object, _installmentRepositoryMock.Object);
//    }

//    [Test]
//    public async Task CreateNewPolicyAsync_Calling_AddAsync()
//    {
//        var policyEntity = new PolicyEntity(
//                                policyID: 1,
//                                description: string.Empty,
//                                cpf: "12345678900",
//                                situation: "ATIVO",
//                                totalPrize: 500.00M,
//                                installments: new Collection<InstallmentEntity>
//                                    {
//                                        new InstallmentEntity(
//                                                installmentID: 1,
//                                                idApolice: 1,
//                                                premium: 100.50M,
//                                                paymentMethod: PaymentMethod.CARTAO,
//                                                paymentDate: new DateOnly(2023, 4, 2),
//                                                situation: "PAGO",
//                                                policy: null
//                                                )
//                                    });

//        _policyRepositoryMock.Setup(s => s.AddAsync(policyEntity)).Returns(Task.CompletedTask);

//        await _policyDomainService.CreateNewPolicyAsync(policyEntity);

//        _policyRepositoryMock.Verify(v => v.AddAsync(policyEntity), Times.Once);
//    }

//    [Test]
//    public async Task RetrievePolicyByIdAsync_Calling_GetByIdAsync()
//    {
//        var policyId = 1;

//        var policyEntity = new PolicyEntity(
//                                policyID: policyId,
//                                description: string.Empty,
//                                cpf: "12345678900",
//                                situation: "ATIVO",
//                                totalPrize: 500.00M,
//                                installments: new Collection<InstallmentEntity>()
//                            );

//        _policyRepositoryMock.Setup(s => s.GetByIdAsync(policyId)).ReturnsAsync(policyEntity);

//        var result = await _policyDomainService.RetrievePolicyByIdAsync(policyId);

//        _policyRepositoryMock.Verify(v => v.GetByIdAsync(policyId), Times.Once);

//        Assert.AreEqual(policyEntity, result);
//    }

//    [Test]
//    public async Task RetrieveAllPoliciesAsync_Calling_GetAllAsync()
//    {
//        var policyEntities = new List<PolicyEntity>()
//        {
//            new PolicyEntity(
//                policyID: 1,
//                description: string.Empty,
//                cpf: "12345678900",
//                situation: "ATIVO",
//                totalPrize: 500.00M,
//                installments: new Collection<InstallmentEntity>()
//            ),
//            new PolicyEntity(
//                policyID: 2,
//                description: string.Empty,
//                cpf: "98765432100",
//                situation: "ATIVO",
//                totalPrize: 200.00M,
//                installments: new Collection<InstallmentEntity>()
//            )
//        };

//        var skip = 0;
//        var take = 10;

//        _policyRepositoryMock.Setup(s => s.GetAllAsync(skip, take)).ReturnsAsync(policyEntities);

//        var result = await _policyDomainService.RetrieveAllPoliciesAsync(skip, take);

//        _policyRepositoryMock.Verify(v => v.GetAllAsync(skip, take), Times.Once);

//        Assert.AreEqual(policyEntities, result);
//    }

//    [Test]
//    public void RegisterPaymentForPolicyAsync_ThrowsNotFoundException()
//    {
//        InstallmentEntity installmentEntity = null;

//        _installmentRepositoryMock
//            .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
//                .ReturnsAsync(installmentEntity);

//        Assert.ThrowsAsync<PolicyNotFoundException>(async () => await _policyDomainService.RegisterPaymentForPolicyAsync(1, new DateOnly()));
//    }

//    [Test]
//    public async Task RegisterPaymentForPolicyAsync_Interest_Free()
//    {
//        var installmentEntity =
//            new InstallmentEntity(
//                    installmentID: 1,
//                    idApolice: 1234,
//                    premium: 1000.0m,
//                    paymentMethod: PaymentMethod.BOLETO,
//                    paymentDate: new DateOnly(2023, 04, 02),
//                    situation: "PAGO",
//                    policy: null);

//        _installmentRepositoryMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(installmentEntity);

//        await _policyDomainService.RegisterPaymentForPolicyAsync(1, new DateOnly(2023, 04, 02));

//        Assert.AreEqual(installmentEntity.Situation, "PAGO");
//        Assert.IsNull(installmentEntity.Interest);

//        _installmentRepositoryMock.Verify(x => x.UpdateAsync(installmentEntity), Times.Once);
//    }

//    [Test]
//    public async Task RegisterPaymentForPolicyAsync_With_Interest()
//    {
//        var premium = 1000m;

//        var installmentEntity =
//            new InstallmentEntity(
//                    installmentID: 1,
//                    idApolice: 1234,
//                    premium: premium,
//                    paymentMethod: PaymentMethod.BOLETO,
//                    paymentDate: new DateOnly(2023, 3, 1),
//                    situation: "PAGO",
//                    policy: null);

//        _installmentRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(installmentEntity);

//        var paidDate = new DateOnly(2023, 4, 2);

//        await _policyDomainService.RegisterPaymentForPolicyAsync(1, paidDate);

//        var calcFees = premium * 0.01m * (paidDate.DayNumber - installmentEntity.PaymentDate.Value.DayNumber);

//        Assert.AreEqual(installmentEntity.Situation, "PAGO");
//        Assert.IsNotNull(installmentEntity.Interest);
//        Assert.AreEqual(installmentEntity.Interest, calcFees);

//        _installmentRepositoryMock.Verify(v => v.UpdateAsync(installmentEntity), Times.Once);
//    }

//    [Test]
//    public async Task UpdatePolicyAsync_WithValidPolicy_UpdatePolicy()
//    {
//        var policyEntity = new PolicyEntity(
//                                policyID: 1,
//                                description: string.Empty,
//                                cpf: "12345678900",
//                                situation: "ATIVO",
//                                totalPrize: 1000.0M,
//                                installments: new Collection<InstallmentEntity>
//                                    {
//                                        new InstallmentEntity(
//                                                installmentID: 1,
//                                                idApolice: 1,
//                                                premium: 500.0m,
//                                                paymentMethod: PaymentMethod.CARTAO,
//                                                paymentDate: DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
//                                                situation: "PAGO",
//                                                policy: null
//                                                ),

//                                        new InstallmentEntity(
//                                                installmentID: 2,
//                                                idApolice: 1,
//                                                premium: 500.0m,
//                                                paymentMethod: PaymentMethod.DINHEIRO,
//                                                paymentDate: DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
//                                                situation: "PAGO",
//                                                policy: null
//                                                )
//                                    });

//        _policyRepositoryMock.Setup(s => s.GetByIdAsync(policyEntity.EntityID)).ReturnsAsync(policyEntity);

//        await _policyDomainService.UpdatePolicyAsync(policyEntity);

//        _policyRepositoryMock.Verify(v => v.UpdateAsync(It.IsAny<PolicyEntity>()), Times.Once);
//    }

//    [Test]
//    public void UpdatePolicyAsync_WithInvalidPolicy_UpdatePolicy()
//    {
//        // Arrange
//        var policyEntity = new PolicyEntity(
//                                policyID: 1,
//                                description: string.Empty,
//                                cpf: "12345678900",
//                                situation: "ATIVO",
//                                totalPrize: 1000.0M,
//                                installments: new Collection<InstallmentEntity>
//                                    {
//                                        new InstallmentEntity(
//                                                installmentID: 1,
//                                                idApolice: 1,
//                                                premium: 500.0m,
//                                                paymentMethod: PaymentMethod.CARTAO,
//                                                paymentDate: DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
//                                                situation: "PAGO",
//                                                policy: null
//                                                ),

//                                        new InstallmentEntity(
//                                                installmentID: 2,
//                                                idApolice: 1,
//                                                premium: 500.0m,
//                                                paymentMethod: PaymentMethod.DINHEIRO,
//                                                paymentDate: DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
//                                                situation: "PAGO",
//                                                policy: null
//                                                )
//                                    });

//        _policyRepositoryMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(policyEntity);

//        var ex = Assert.ThrowsAsync<PolicyNotFoundException>(async () => await _policyDomainService.UpdatePolicyAsync(policyEntity));

//        Assert.AreEqual("Apólice não encontrada no banco de dados.", ex.Message);
//    }
//}