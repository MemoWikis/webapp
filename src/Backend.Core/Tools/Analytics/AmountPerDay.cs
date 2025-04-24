public class AmountPerDay
{
    public DateTime DateTime;
    public int Value;

    public static List<AmountPerDay> FillUpDatesWithZeros(
        List<AmountPerDay> list,
        DateTime from,
        DateTime to)
    {
        if (from.Date > to.Date)
            return list;
        var curDay = from.Date;
        while (curDay <= to.Date)
        {
            if (list.Find(i => i.DateTime == curDay) == null)
                list.Add(new AmountPerDay
                {
                    DateTime = curDay,
                    Value = 0
                });
            curDay = curDay.AddDays(1);
        }

        return list.OrderBy(i => i.DateTime).ToList();
    }
}