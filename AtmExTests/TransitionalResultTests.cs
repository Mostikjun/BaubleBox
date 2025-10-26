using AtmEx;

namespace AtmExTests;

public class TransitionalResultTests
{
    [Fact]
    public void Ctor_GivenInvalidBanknote_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(-1, 1, []));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(0, 1, []));

        var prevResult = new TransitionalResult(10, 1, [new(10, 1000)]);
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(-1, 1, prevResult));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(0, 1, prevResult));
    }

    [Fact]
    public void Ctor_GivenInvalidAllocatedNumber_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, -1, []));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, 0, []));

        var prevResult = new TransitionalResult(10, 1, [new(10, 1000)]);
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, -1, prevResult));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TransitionalResult(100, 0, prevResult));
    }

    [Fact]
    public void Banknote_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var result = new TransitionalResult(100, 5, []);

        Assert.Equal(100, result.Banknote);
    }

    [Fact]
    public void AllocatedNumber_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var result = new TransitionalResult(100, 5, []);

        Assert.Equal(5, result.AllocatedNumber);
    }

    [Fact]
    public void GetRemainingBanknoteNumber_GivenExistingBanknote_ReturnsRemainingNumber()
    {
        var result = new TransitionalResult(
            100,
            5,
            [
                new (5, 100),
                new (10, 200),
                new (50, 300),
                new (100, 400)
            ]);

        Assert.Equal(100, result.GetRemainingBanknoteNumber(5));
        Assert.Equal(200, result.GetRemainingBanknoteNumber(10));
        Assert.Equal(300, result.GetRemainingBanknoteNumber(50));
        Assert.Equal(399, result.GetRemainingBanknoteNumber(100));
    }

    [Fact]
    public void GetRemainingBanknoteNumber_GivenNonExistingBanknote_ThrowsException()
    {
        var result = new TransitionalResult(
            100,
            5,
            [
                new (5, 100),
                new (10, 200),
                new (50, 300),
                new (100, 400)
            ]);

        Assert.Throws<ArgumentException>(() => result.GetRemainingBanknoteNumber(25));
    }
}