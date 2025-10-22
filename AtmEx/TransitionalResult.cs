namespace AtmEx;

public class TransitionalResult
{
    public TransitionalResult(int banknote, int allocatedNumber, int remainingNumber)
    {
        banknote.AssertPositive(nameof(banknote));
        allocatedNumber.AssertPositive(nameof(allocatedNumber));
        remainingNumber.AssertNotNegative(nameof(remainingNumber));

        Banknote = banknote;
        AllocatedNumber = allocatedNumber;
        RemainingNumber = remainingNumber;
    }

    public int Banknote { get; }

    public int AllocatedNumber { get; }

    public int RemainingNumber { get; }
}
