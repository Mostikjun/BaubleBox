namespace AtmEx;

public class Atm
{
    private readonly IList<BanknoteSlot> _slots;

    public Atm(IEnumerable<BanknoteSlot> slots)
    {
        var hasDuplicates = slots.GroupBy(x => x.Banknote).Any(x => x.Count() > 1);
        if (hasDuplicates)
        {
            throw new ArgumentException(
                nameof(slots),
                $"Argument '{nameof(slots)}' contains banknote duplicates.");
        }

        _slots = slots
            .OrderBy(x => x.Banknote)
            .ToList();
    }

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

    private TransitionalResult?[] GetTransitionalDispenses(int amount)
    {
        var transitionalAmount = 1;
        var transitionalResults = new TransitionalResult?[amount + 1];

        while (transitionalAmount <= amount)
        {
            foreach (var slot in _slots)
            {
                if (transitionalAmount < slot.Banknote)
                {
                    break;
                }

                if (transitionalAmount == slot.Banknote)
                {
                    if (slot.Number > 0)
                    {
                        transitionalResults[transitionalAmount] = new TransitionalResult(
                            slot.Banknote,
                            1,
                            _slots);
                    }

                    break;
                }

                var prevTransitionalResult = transitionalResults[transitionalAmount - slot.Banknote];
                if (prevTransitionalResult == null)
                {
                    continue;
                }

                var newAllocatedNumber = prevTransitionalResult.AllocatedNumber + 1;

                var curTransitionalResult = transitionalResults[transitionalAmount];
                if (curTransitionalResult != null
                    && curTransitionalResult.AllocatedNumber < newAllocatedNumber)
                {
                    continue;
                }

                var remaining = prevTransitionalResult.GetRemainingBanknoteNumber(slot.Banknote);
                if (remaining <= 0)
                {
                    continue;
                }

                transitionalResults[transitionalAmount] = new TransitionalResult(
                    slot.Banknote,
                    newAllocatedNumber,
                    prevTransitionalResult);
            }

            transitionalAmount++;
        }

        return transitionalResults;
    }

    private IEnumerable<BanknoteSlot> GetDispenseSlots(
        int amount,
        TransitionalResult?[] transitionalDispenses)
    {
        var transitionalDispense = transitionalDispenses[amount];

        var dispenseSlots = new List<BanknoteSlot>();
        var transitionalAmount = amount;
        while (transitionalDispense != null)
        {
            dispenseSlots.Add(new(transitionalDispense!.Banknote, 1));

            transitionalAmount -= transitionalDispense.Banknote;

            transitionalDispense = transitionalDispenses[transitionalAmount];
        }

        return dispenseSlots
            .GroupBy(
                slot => slot.Banknote,
                (banknote, groupedSlots) => new BanknoteSlot(banknote, groupedSlots.Count()))
            .OrderByDescending(slot => slot.Banknote);
    }
}
