namespace AtmEx;

public static class ArgValidationExtensions
{
    public static void AssertPositive(this int value, string name)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                name,
                $"Argument '{name}' must be greater than 0");
        }
    }

    public static void AssertNotNegative(this int value, string name)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                name,
                $"Argument '{name}' must be greater than or equal to 0");
        }
    }

}
