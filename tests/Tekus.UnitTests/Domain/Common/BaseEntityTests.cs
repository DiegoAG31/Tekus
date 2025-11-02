using FluentAssertions;
using Tekus.Core.Domain.Common;

namespace Tekus.UnitTests.Domain.Common;

/// <summary>
/// Tests for BaseEntity following TDD approach
/// </summary>
public class BaseEntityTests
{
    // Helper class for testing
    private class TestEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void NewEntity_ShouldHaveDefaultValues()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().Be(Guid.Empty);
        entity.CreatedAt.Should().Be(default);
        entity.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void CreatedAt_WhenSet_ShouldRetainValue()
    {
        // Arrange
        var entity = new TestEntity();
        var now = DateTime.UtcNow;

        // Act
        entity.CreatedAt = now;

        // Assert
        entity.CreatedAt.Should().Be(now);
    }

    [Fact]
    public void UpdatedAt_WhenSet_ShouldRetainValue()
    {
        // Arrange
        var entity = new TestEntity();
        var now = DateTime.UtcNow;

        // Act
        entity.UpdatedAt = now;

        // Assert
        entity.UpdatedAt.Should().Be(now);
    }

    [Fact]
    public void TwoEntitiesWithSameId_ShouldBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity { Id = id };
        var entity2 = new TestEntity { Id = id };

        // Act & Assert
        entity1.Should().Be(entity2);
        (entity1 == entity2).Should().BeTrue();
    }

    [Fact]
    public void TwoEntitiesWithDifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        var entity1 = new TestEntity { Id = Guid.NewGuid() };
        var entity2 = new TestEntity { Id = Guid.NewGuid() };

        // Act & Assert
        entity1.Should().NotBe(entity2);
        (entity1 != entity2).Should().BeTrue();
    }

    [Fact]
    public void EntityWithEmptyId_ComparedToNull_ShouldNotBeEqual()
    {
        // Arrange
        var entity = new TestEntity();

        // Act & Assert
        entity.Should().NotBeNull();
        (entity == null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ForSameId_ShouldBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity { Id = id };
        var entity2 = new TestEntity { Id = id };

        // Act & Assert
        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ForDifferentIds_ShouldBeDifferent()
    {
        // Arrange
        var entity1 = new TestEntity { Id = Guid.NewGuid() };
        var entity2 = new TestEntity { Id = Guid.NewGuid() };

        // Act & Assert
        entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }
}