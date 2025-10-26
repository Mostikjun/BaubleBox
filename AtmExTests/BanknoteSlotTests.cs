using AtmEx;

namespace AtmExTests;

public class BanknoteSlotTests
{
    [Fact]
    public void Ctor_GivenInvalidBanknote_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BanknoteSlot(-1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BanknoteSlot(0, 1));
    }

    [Fact]
    public void Ctor_GivenInvalidNumber_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BanknoteSlot(100, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BanknoteSlot(100, 0));
    }

    [Fact]
    public void Banknote_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var slot = new BanknoteSlot(100, 5);

        Assert.Equal(100, slot.Banknote);
    }

    [Fact]
    public void Number_GivenCtorArgument_ReturnsCtorArgumentValue()
    {
        var slot = new BanknoteSlot(100, 5);

        Assert.Equal(5, slot.Number);
    }
}