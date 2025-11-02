using FluentAssertions;
using Tekus.Core.Domain.Common;

namespace Tekus.UnitTests.Domain.Common;

/// <summary>
/// Tests for PagedResult class
/// </summary>
public class PagedResultTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var totalCount = 10;
        var pageNumber = 1;
        var pageSize = 3;

        // Act
        var result = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(10);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(3);
        result.TotalPages.Should().Be(4); // 10 / 3 = 3.33 -> 4
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void Create_WithLastPage_ShouldHaveCorrectFlags()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var totalCount = 10;
        var pageNumber = 4;
        var pageSize = 3;

        // Act
        var result = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        result.TotalPages.Should().Be(4);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void Create_WithMiddlePage_ShouldHaveBothFlags()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var totalCount = 10;
        var pageNumber = 2;
        var pageSize = 3;

        // Act
        var result = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyItems_ShouldSucceed()
    {
        // Arrange
        var items = new List<string>();
        var totalCount = 0;
        var pageNumber = 1;
        var pageSize = 10;

        // Act
        var result = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void Create_WithSinglePage_ShouldHaveNoNavigation()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };
        var totalCount = 2;
        var pageNumber = 1;
        var pageSize = 10;

        // Act
        var result = new PagedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        result.TotalPages.Should().Be(1);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }

    [Theory]
    [InlineData(10, 3, 4)]  // 10 items, 3 per page = 4 pages
    [InlineData(9, 3, 3)]   // 9 items, 3 per page = 3 pages
    [InlineData(10, 5, 2)]  // 10 items, 5 per page = 2 pages
    [InlineData(1, 10, 1)]  // 1 item, 10 per page = 1 page
    public void TotalPages_ShouldCalculateCorrectly(int totalCount, int pageSize, int expectedPages)
    {
        // Arrange & Act
        var result = new PagedResult<string>(new List<string>(), totalCount, 1, pageSize);

        // Assert
        result.TotalPages.Should().Be(expectedPages);
    }
}