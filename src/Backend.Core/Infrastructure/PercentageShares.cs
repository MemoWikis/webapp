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

    public static int[] FromDecimalSharesToIntPercentage(double[] values)
    {
        int[] results = new int[values.Length];
        double error = 0;

        // First pass - calculate initial percentages with error correction
        for (int i = 0; i < values.Length; i++)
        {
            double val = values[i] * 100;
            int percent = (int)Math.Round(val + error);

            // Ensure we never go below 0%
            percent = Math.Max(0, percent);

            error += val - percent;
            if (Math.Abs(error) >= 0.5)
            {
                int sign = Math.Sign(error);
                percent += sign;
                error -= sign;

                // Double-check after adjustment
                percent = Math.Max(0, percent);
            }

            results[i] = percent;
        }

        // Ensure total adds up to 100%
        int total = results.Sum();
        if (total != 100 && total > 0)
        {
            // Adjust the largest value to make total 100%
            int indexOfMax = Array.IndexOf(results, results.Max());
            results[indexOfMax] += (100 - total);
        }

        return results;
    }
}