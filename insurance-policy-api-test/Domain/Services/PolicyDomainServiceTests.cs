using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Entities.Installment;
using insurance_policy_api_domain.Entities;
using insurance_policy_api_domain.Exceptions;
using insurance_policy_api_domain.Services;
using Moq;
using NUnit.Framework;

namespace insurance_policy_api_test.Domain.Services;

[TestFixture]
public class PolicyDomainServiceTests
{
    private Mock<IPolicyRepository> _mockPolicyRepository;
    private PolicyDomainService _policyDomainService;

    [SetUp]
    public void Setup()
    {
        _mockPolicyRepository = new Mock<IPolicyRepository>();
        _policyDomainService = new PolicyDomainService(_mockPolicyRepository.Object);
    }

    [Test]
    public async Task CreateNewPolicyAsync_ValidPolicyEntity_PolicyAdded()
    {
        // Arrange
        var policyEntity = new PolicyEntity(1, "Policy Description", "1234567890", "Active", 1000, new List<InstallmentEntity>());

        // Act
        await _policyDomainService.CreateNewPolicyAsync(policyEntity);

        // Assert
        _mockPolicyRepository.Verify(r => r.AddAsync(policyEntity), Times.Once);
    }

    [Test]
    public async Task RetrievePolicyByIdAsync_ExistingPolicyId_ReturnsPolicyEntity()
    {
        // Arrange
        var policyId = 1;
        var expectedPolicy = new PolicyEntity(policyId, "Policy Description", "1234567890", "Active", 1000, new List<InstallmentEntity>());

        _mockPolicyRepository.Setup(r => r.GetByIdAsync(policyId))
            .ReturnsAsync(expectedPolicy);

        // Act
        var retrievedPolicy = await _policyDomainService.RetrievePolicyByIdAsync(policyId);

        // Assert
        Assert.AreEqual(expectedPolicy, retrievedPolicy);
    }

    [Test]
    public async Task RetrievePolicyByIdAsync_NonExistingPolicyId_ReturnsNull()
    {
        // Arrange
        var policyId = 1;

        _mockPolicyRepository.Setup(r => r.GetByIdAsync(policyId))
            .ReturnsAsync((PolicyEntity)null);

        // Act
        var retrievedPolicy = await _policyDomainService.RetrievePolicyByIdAsync(policyId);

        // Assert
        Assert.IsNull(retrievedPolicy);
    }

    [Test]
    public async Task RetrieveAllPoliciesAsync_ValidParameters_ReturnsPolicies()
    {
        // Arrange
        var skip = 0;
        var take = 10;
        var expectedPolicies = new List<PolicyEntity>
        {
            new PolicyEntity(1, "Policy 1", "1234567890", "Active", 1000, new List<InstallmentEntity>()),
            new PolicyEntity(2, "Policy 2", "0987654321", "Inactive", 2000, new List<InstallmentEntity>())
        };

        _mockPolicyRepository.Setup(r => r.GetAllAsync(skip, take))
            .ReturnsAsync(expectedPolicies);

        // Act
        var retrievedPolicies = await _policyDomainService.RetrieveAllPoliciesAsync(skip, take);

        // Assert
        CollectionAssert.AreEqual(expectedPolicies, retrievedPolicies.ToList());
    }

    [Test]
    public async Task UpdatePolicyAsync_ExistingPolicy_PolicyUpdated()
    {
        // Arrange
        var policyId = 1;
        var policyEntityToUpdate = new PolicyEntity(policyId, "Updated Policy", "1234567890", "Active", 1000, new List<InstallmentEntity>());
        var existingPolicy = new PolicyEntity(policyId, "Existing Policy", "1234567890", "Active", 1000, new List<InstallmentEntity>());

        _mockPolicyRepository.Setup(r => r.GetByIdAsync(policyEntityToUpdate.EntityID))
            .ReturnsAsync(existingPolicy);

        // Act
        await _policyDomainService.UpdatePolicyAsync(policyEntityToUpdate);

        // Assert
        _mockPolicyRepository.Verify(r => r.UpdateAsync(policyEntityToUpdate), Times.Once);
    }

    [Test]
    public void UpdatePolicyAsync_NonExistingPolicy_ThrowsPolicyNotFoundException()
    {
        // Arrange
        var policyId = 1;
        var policyEntityToUpdate = new PolicyEntity(policyId, "Updated Policy", "1234567890", "Active", 1000, new List<InstallmentEntity>());

        _mockPolicyRepository.Setup(r => r.GetByIdAsync(policyEntityToUpdate.EntityID))
            .ReturnsAsync((PolicyEntity)null);

        // Act
        Func<Task> updateAction = async () => await _policyDomainService.UpdatePolicyAsync(policyEntityToUpdate);

        // Assert
        Assert.That(updateAction, Throws.TypeOf<PolicyNotFoundException>()
            .With.Message.EqualTo($"The policy with ID {policyEntityToUpdate.EntityID} was not found in the database during the policy update request."));

        // Verifies
        _mockPolicyRepository.Verify(r => r.UpdateAsync(It.IsAny<PolicyEntity>()), Times.Never);
    }
}
