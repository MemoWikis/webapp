using System;
using System.Collections.Generic;
using System.Linq;

public class ValueWithResultAction
{
    public int AbsoluteValue;
    public Action<int> ActionForPercentage;
}

public class PercentageShares
{
    public static void FromAbsoluteShares(IList<ValueWithResultAction> values)
    {
        var percentageResults = FromAbsoluteShares(values.Select(t => t.AbsoluteValue).ToArray());

        for (var i = 0; i < values.Count; i++)
            values[i].ActionForPercentage(percentageResults[i]);
    }

    public static int[] FromAbsoluteShares(int[] values)
    {
        var total = (double)values.Sum();
        //Sum of all given absolute shares establishes 100%
        var valuesAsDoubles = values.Select(v => v / total).ToArray();
        return FromDecimalSharesToIntPercentage(valuesAsDoubles);
    }

    //https://stackoverflow.com/questions/20137562/round-decimals-and-convert-to-in-c-sharp
    //Ensure that values round up correctly
    public static int[] FromDecimalSharesToIntPercentage(double[] values)
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
