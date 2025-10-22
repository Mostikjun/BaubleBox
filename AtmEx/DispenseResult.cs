namespace AtmEx;

public class DispenseResult
{
    public DispenseResult(int totalNumber, IEnumerable<BanknoteSlot> dispense)
    {
        totalNumber.AssertNotNegative(nameof(totalNumber));

        TotalNumber = totalNumber;
        Dispense = dispense.ToList().AsReadOnly();
    }

    public int TotalNumber { get; }

    public IReadOnlyCollection<BanknoteSlot> Dispense { get; }

    public bool CanDispense => TotalNumber > 0;

    public static DispenseResult Failure => new DispenseResult(0, []);
}
