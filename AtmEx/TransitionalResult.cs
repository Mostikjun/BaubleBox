namespace AtmEx;

public class TransitionalResult
{
    private readonly IReadOnlyDictionary<int, BanknoteSlot> _remainingBanknotes;

    public TransitionalResult(
        int banknote,
        int allocatedNumber,
        IEnumerable<BanknoteSlot> slots)
    {
        banknote.AssertPositive(nameof(banknote));
        allocatedNumber.AssertPositive(nameof(allocatedNumber));

        Banknote = banknote;
        AllocatedNumber = allocatedNumber;
        _remainingBanknotes = slots
            .ToDictionary(
                x => x.Banknote,
                x => x.Banknote == banknote
                    ? new BanknoteSlot(x.Banknote, x.Number - 1)
                    : x)
            .AsReadOnly();
    }

    public TransitionalResult(
        int banknote,
        int allocatedNumber,
        TransitionalResult prevResult)
    {
        banknote.AssertPositive(nameof(banknote));
        allocatedNumber.AssertPositive(nameof(allocatedNumber));

        Banknote = banknote;
        AllocatedNumber = allocatedNumber;
        _remainingBanknotes = prevResult._remainingBanknotes
            .ToDictionary(
                x => x.Key,
                x => x.Value.Banknote == banknote
                    ? new BanknoteSlot(x.Value.Banknote, x.Value.Number - 1)
                    : x.Value)
            .AsReadOnly();
    }

    public int Banknote { get; }

    public int AllocatedNumber { get; }

    public int GetRemainingBanknoteNumber(int banknote)
    {
        if (!_remainingBanknotes.TryGetValue(banknote, out var slot))
        {
            throw new ArgumentException(
                nameof(banknote),
                $"Cannot find banknote {banknote}.");
        }

        return slot.Number;
    }
}
