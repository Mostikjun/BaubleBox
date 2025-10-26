using System.Diagnostics;
using AtmEx;

namespace AtmExTests;

public class AtmTests
{
    [Fact]
    public void Ctor_GivenDuplicateBanknotes_ThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Atm(
            [
                new (5, 1000),
                new (10, 1000),
                new (5, 1000), // Duplicate
                new (50, 1000),
                new (100, 1000)
            ]));
    }

    [Fact]
    public void Dispense_GivenNoBanknoteSlots_ReturnsFailure()
    {
        var atm = new Atm([]);

        var actual = atm.Dispense(17);

        Assert.Equivalent(
            DispenseResult.Failure,
            actual,
            true);
    }

    [Fact]
    public void Dispense_GivenUnOrderedBanknoteSlots_ReturnsValidDispense()
    {
        var atm = new Atm(
            [
                new (50, 1000),
                new (5, 1000),
                new (100, 1000),
                new (10, 1000)
            ]);

        var actual = atm.Dispense(45);

        Assert.Equivalent(
            new DispenseResult(5, [new(10, 4), new(5, 1)]),
            actual,
            true);
    }

    [Fact]
    public void Dispense_GivenUnresolvableAmount_ReturnsFailure()
    {
        var atm = new Atm(
            [
                new (5, 1000),
                new (10, 1000),
                new (50, 1000),
                new (100, 1000)
            ]);

        var actual = atm.Dispense(17);

        Assert.Equivalent(
            DispenseResult.Failure,
            actual,
            true);
    }

    [Fact]
    public void Dispense_GivenLimitedNumberOfBanknote_ReturnsAmountUsigOtherAvailableBanknotes()
    {
        var atm = new Atm(
            [
                new (5, 1000),
                new (10, 1),
                new (50, 1000),
                new (100, 1000)
            ]);

        var actual = atm.Dispense(45);

        Assert.Equivalent(
            new DispenseResult(8, [new(10, 1), new(5, 7)]),
            actual,
            true);
    }

    [Fact]
    public void Dispense_GivenAmountDivisableByTwo_ReturnsShorterDispenseThanGreedyAlgorithm()
    {
        var atm = new Atm(
            [
                new (5, 1000),
                new (10, 1000),
                new (40, 1000),
                new (50, 1000),
                new (100, 1000)
            ]);

        var actual = atm.Dispense(80);

        // Amount 80 can be given with less number of samller banknotes: 40 + 40.
        // As opposed to greedy algorithm: 50 + 10 + 10 + 10.
        Assert.Equivalent(
            new DispenseResult(2, [new(40, 2)]),
            actual,
            true);
    }

    [Fact]
    public void Dispense_GivenBigAmount_ReturnsValidDispense()
    {
        var stopwatch = Stopwatch.StartNew();

        var atm = new Atm(
            [
                new (5, 1000),
                new (10, 1000),
                new (50, 1000),
                new (100, 1000),
                new (1000, 10_000)
            ]);

        var actual = atm.Dispense(10_000_150);

        stopwatch.Stop();
        Debug.WriteLine($"Dispense execution time: {stopwatch.Elapsed.TotalSeconds} sec");

        Assert.Equivalent(
            new DispenseResult(10_002, [new(1000, 10_000), new(100, 1), new(50, 1)]),
            actual,
            true);
    }
}
