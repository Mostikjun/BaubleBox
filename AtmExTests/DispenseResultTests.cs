using AtmEx;

namespace AtmExTests;

public class DispenseResultTests
{
    [Fact]
    public void Ctor_GivenNegativeTotalNumber_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DispenseResult(-1, []));
    }

    [Fact]
    public void CanDispense_GivenTotalNumberEqualToZero_ReturnsFalse()
    {
        var dispenseResult = new DispenseResult(0, []);
        Assert.False(dispenseResult.CanDispense);
    }

    [Fact]
    public void CanDispense_GivenTotalNumberGreaterThanZero_ReturnsTrue()
    {
        var dispenseResult = new DispenseResult(2, [new BanknoteSlot(10, 1), new BanknoteSlot(5, 1)]);
        Assert.True(dispenseResult.CanDispense);
    }

    [Fact]
    public void Failure_ReturnsResultThatCannotBeDispensed()
    {
        var failure = DispenseResult.Failure;

        Assert.False(failure.CanDispense);
        Assert.Equal(0, failure.TotalNumber);
        Assert.Empty(failure.Dispense);
    }
}
