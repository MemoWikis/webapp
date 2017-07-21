using System;
using System.Collections.Generic;
using System.Linq;

public class PercentAction
{
    public int Value;
    public Action<int> Action;
}

public class Percents
{
    public static void FromIntsd(IList<PercentAction> values)
    {
        var results = FromInts(values.Select(t => t.Value).ToArray());

        for (var i = 0; i < values.Count; i++)
            values[i].Action(results[i]);
    }

    public static int[] FromInts(int[] values)
    {
        var total = (double)values.Sum();
        var valuesAsDoubles = values.Select(v => v / total).ToArray();
        return FromInts(valuesAsDoubles);
    }

    //https://stackoverflow.com/questions/20137562/round-decimals-and-convert-to-in-c-sharp
    public static int[] FromInts(double[] values)
    {
        int[] results = new int[values.Length];
        double error = 0;
        for (int i = 0; i < values.Length; i++)
        {
            double val = values[i] * 100;
            int percent = (int)Math.Round(val + error);
            error += val - percent;
            if (Math.Abs(error) >= 0.5)
            {
                int sign = Math.Sign(error);
                percent += sign;
                error -= sign;
            }
            results[i] = percent;
        }

        return results;
    }
}
