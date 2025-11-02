using FluentAssertions;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;
using Tekus.Core.Domain.Exceptions;
using Tekus.Core.Domain.ValueObjects;

namespace Tekus.UnitTests.Domain.Aggregates;

/// <summary>
/// Tests for Provider Custom Fields functionality
/// </summary>
public class ProviderCustomFieldsTests
{
    [Fact]
    public void AddCustomField_WithValidData_ShouldSucceed()
    {
        // Arrange
        var provider = CreateValidProvider();
        var fieldName = "Número de contacto en marte";
        var fieldValue = "+1-234-567-8900";
        var fieldType = "text";

        // Act
        provider.AddCustomField(fieldName, fieldValue, fieldType);

        // Assert
        provider.CustomFields.Should().HaveCount(1);
        var field = provider.CustomFields.First();
        field.FieldName.Should().Be(fieldName);
        field.FieldValue.Should().Be(fieldValue);
        field.FieldType.Should().Be(fieldType);
        provider.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void AddCustomField_WithEmptyFieldName_ShouldThrowDomainException()
    {
        // Arrange
        var provider = CreateValidProvider();

        // Act
        Action act = () => provider.AddCustomField("", "value", "text");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Field name cannot be empty*");
    }

    [Fact]
    public void AddCustomField_WithDuplicateName_ShouldUpdateExisting()
    {
        // Arrange
        var provider = CreateValidProvider();
        var fieldName = "Test Field";
        provider.AddCustomField(fieldName, "old value", "text");

        // Act
        provider.AddCustomField(fieldName, "new value", "number");

        // Assert
        provider.CustomFields.Should().HaveCount(1);
        var field = provider.CustomFields.First();
        field.FieldValue.Should().Be("new value");
        field.FieldType.Should().Be("number");
    }

    [Fact]
    public void AddMultipleCustomFields_ShouldSucceed()
    {
        // Arrange
        var provider = CreateValidProvider();

        // Act
        provider.AddCustomField("Field1", "Value1", "text");
        provider.AddCustomField("Field2", "Value2", "number");
        provider.AddCustomField("Field3", "Value3", "date");

        // Assert
        provider.CustomFields.Should().HaveCount(3);
    }

    [Fact]
    public void RemoveCustomField_WithValidId_ShouldSucceed()
    {
        // Arrange
        var provider = CreateValidProvider();
        provider.AddCustomField("Test Field", "Test Value", "text");
        var fieldId = provider.CustomFields.First().Id;

        // Act
        provider.RemoveCustomField(fieldId);

        // Assert
        provider.CustomFields.Should().BeEmpty();
        provider.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void RemoveCustomField_WithNonExistentId_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var provider = CreateValidProvider();
        var nonExistentId = Guid.NewGuid();

        // Act
        Action act = () => provider.RemoveCustomField(nonExistentId);

        // Assert
        act.Should().Throw<EntityNotFoundException>()
            .WithMessage($"*{nonExistentId}*not found*");
    }

    [Fact]
    public void CustomFields_ShouldBeReadOnly()
    {
        // Arrange
        var provider = CreateValidProvider();

        // Assert
        provider.CustomFields.Should().BeAssignableTo<IReadOnlyCollection<ProviderCustomField>>();
    }

    // Helper method
    private Provider CreateValidProvider()
    {
        var nit = Nit.Create("900123456-7");
        var name = "Test Provider";
        var email = Email.Create("test@tekus.com");
        return Provider.Create(nit, name, email);
    }
}