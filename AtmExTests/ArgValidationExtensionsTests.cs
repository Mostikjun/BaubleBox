using AtmEx;

namespace AtmExTests;

public class ArgValidationExtensionsTests
{
    [Fact]
    public void AssertPositive_GivenPositiveValue_DoesNotThrow()
    {
        1.AssertPositive("test");
    }

    [Fact]
    public void AssertPositive_GivenZero_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 0.AssertPositive("test"));
    }

    [Fact]
    public void AssertPositive_GivenNegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => (-1).AssertPositive("test"));
    }

    [Fact]
    public void AssertNotNegative_GivenPositiveValue_DoesNotThrow()
    {
        1.AssertNotNegative("test");
    }

    [Fact]
    public void AssertNotNegative_GivenZero_DoesNotThrow()
    {
        0.AssertNotNegative("test");
    }

    [Fact]
    public void AssertNotNegative_GivenNegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => (-1).AssertNotNegative("test"));
    }
}