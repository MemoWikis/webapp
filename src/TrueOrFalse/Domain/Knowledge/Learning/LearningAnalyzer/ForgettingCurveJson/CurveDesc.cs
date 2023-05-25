using System.Collections.Generic;
using Seedworks.Lib;

public class CurvesJsonCmd
{
    public CurvesJsonCmd()
    {
        Curves = new List<CurveDesc>();
    }

    public List<CurveDesc> Curves { get; set; }
    public string Interval { get; set; }
    public int IntervalCount { get; set; }

    public void Process()
    {
        foreach (var curve in Curves)
        {
            if (curve.QuestionFeatureId.IsNumeric())
            {
                curve.QuestionFeature = Sl.R<QuestionFeatureRepo>().GetById(curve.QuestionFeatureId.ToInt32());
            }
        }
    }
}

public class CurveDesc
{
    public QuestionFeature QuestionFeature;

    public string ColumnId => ColumnLabel.Replace(" ", "");

    public string ColumnLabel
    {
        get
        {
            var result = "";
            if (QuestionFeature != null && !string.IsNullOrEmpty(QuestionFeature.Name))
            {
                result += " " + QuestionFeature.Name;
            }

            return result;
        }
    }

    public string AnswerFeatureId { get; set; }

    /// <summary>
    ///     Question category, possible values:
    ///     - nobrainer,
    ///     - easy,
    ///     - middle
    ///     - hard
    /// </summary>
    public string QuestionFeatureId { get; set; }
}