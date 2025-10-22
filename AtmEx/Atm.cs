namespace AtmEx;

public class Atm
{
    public Atm(IEnumerable<BanknoteSlot> slots)
    {
        var sortedByBanknoteAsc = slots.ToList();
        sortedByBanknoteAsc.Sort((x, y) =>
            x.Banknote < y.Banknote
                ? -1
                : x.Banknote > y.Banknote
                    ? 1
                    : 0);
        Slots = sortedByBanknoteAsc.AsReadOnly();
    }

    public IReadOnlyList<BanknoteSlot> Slots { get; }

    public DispenseResult Dispense(int amount)
    {
        amount.AssertNotNegative(nameof(amount));

        if (amount == 0)
        {
            return DispenseResult.Failure;
        }

        var transitionalDispenses = GetTransitionalDispenses(amount);

        var dispenseSlots = GetDispenseSlots(amount, transitionalDispenses);

        return dispenseSlots.Any()
            ? new DispenseResult(dispenseSlots.Sum(slot => slot.Number), dispenseSlots)
            : DispenseResult.Failure;
    }

    private IDictionary<int, TransitionalResult> GetTransitionalDispenses(int amount)
    {
        var transitionalAmount = 1;
        var transitionalResults = new Dictionary<int, TransitionalResult>();

        while (transitionalAmount <= amount)
        {
            foreach (var slot in Slots)
            {
                if (transitionalAmount < slot.Banknote)
                {
                    break;
                }

                if (transitionalAmount == slot.Banknote)
                {
                    if (slot.Number > 0)
                    {
                        transitionalResults[transitionalAmount] = new TransitionalResult(slot.Banknote, 1, slot.Number - 1);
                    }

                    break;
                }

                var hasPrevTransitionalResult = transitionalResults.TryGetValue(
                    transitionalAmount - slot.Banknote,
                    out var prevTransitionalResult);
                if (!hasPrevTransitionalResult)
                {
                    continue;
                }

                var newAllocatedNumber = prevTransitionalResult!.AllocatedNumber + 1;

                var hasCurrentTransitionalResult = transitionalResults.TryGetValue(
                    transitionalAmount,
                    out var curTransitionalResult);
                if (hasCurrentTransitionalResult
                    && curTransitionalResult!.AllocatedNumber < newAllocatedNumber)
                {
                    continue;
                }

                var remaining = GetRemainingBanknoteNumber(
                    slot.Banknote,
                    transitionalAmount,
                    transitionalResults);
                if (remaining <= 0)
                {
                    continue;
                }

                transitionalResults[transitionalAmount] = new TransitionalResult(
                    slot.Banknote,
                    newAllocatedNumber,
                    remaining - 1);
            }

            transitionalAmount++;
        }

        return transitionalResults;
    }

    private int GetRemainingBanknoteNumber(
        int banknote,
        int amount,
        IDictionary<int, TransitionalResult> transitionalDispenses)
    {
        var remaining = Slots.FirstOrDefault(x => x.Banknote == banknote)?.Number ?? 0;
        if (remaining == 0)
        {
            return 0;
        }

        while (amount > 0)
        {
            amount -= banknote;

            transitionalDispenses.TryGetValue(
                amount,
                out var transitionalDispense);
            if (transitionalDispense == null)
            {
                break;
            }

            if (transitionalDispense.Banknote == banknote)
            {
                remaining = transitionalDispense.RemainingNumber;
                break;
            }
        }

        return remaining;
    }

    private IEnumerable<BanknoteSlot> GetDispenseSlots(
        int amount,
        IDictionary<int, TransitionalResult> transitionalDispenses)
    {
        transitionalDispenses.TryGetValue(
            amount,
            out var transitionalDispense);

        var dispenseSlots = new List<BanknoteSlot>();
        var transitionalAmount = amount;
        while (transitionalDispense != null)
        {
            dispenseSlots.Add(new(transitionalDispense!.Banknote, 1));

            transitionalAmount -= transitionalDispense.Banknote;

            transitionalDispenses.TryGetValue(
                transitionalAmount,
                out transitionalDispense);
        }

        return dispenseSlots
            .GroupBy(
                slot => slot.Banknote,
                (banknote, groupedSlots) => new BanknoteSlot(banknote, groupedSlots.Count()))
            .OrderByDescending(slot => slot.Banknote);
    }
}
