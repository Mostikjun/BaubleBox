namespace AtmEx;

public class BanknoteSlot
{
    public BanknoteSlot(int banknote, int number)
    {
        banknote.AssertPositive(nameof(banknote));
        number.AssertPositive(nameof(number));

        Banknote = banknote;
        Number = number;
    }

    public int Banknote { get; }

    public int Number { get; }
}
