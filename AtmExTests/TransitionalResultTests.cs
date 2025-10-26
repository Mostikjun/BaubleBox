using AtmEx;

namespace AtmExTests;

public class TransitionalResultTests
{
    [Fact]
    public void Ctor_GivenInvalidBanknote_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(-1, 1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(0, 1, 0));
    }

    [Fact]
    public void Ctor_GivenInvalidAllocatedNumber_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, -1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, 0, 0));
    }

    [Fact]
    public void Ctor_GivenInvalidRemainingNumber_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, 5, -1));
    }

    [Fact]
    public void Banknote_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var result = new TransitionalResult(100, 5, 3);

        Assert.Equal(100, result.Banknote);
    }

    [Fact]
    public void AllocatedNumber_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var result = new TransitionalResult(100, 5, 3);

        Assert.Equal(5, result.AllocatedNumber);
    }

    [Fact]
    public void RemainingNumber_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var result = new TransitionalResult(100, 5, 3);

        Assert.Equal(3, result.RemainingNumber);
    }
}