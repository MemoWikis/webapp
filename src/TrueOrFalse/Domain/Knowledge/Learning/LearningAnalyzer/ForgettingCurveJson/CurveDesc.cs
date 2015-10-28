using System;
using System.Collections.Generic;

public class CurvesJsonCmd
{
    public int IntervalCount { get; set; }
    public string Interval { get; set; }
    public List<CurveDesc> Curves { get; set; }

    public CurvesJsonCmd()
    {
        Curves = new List<CurveDesc>();
    }
}


public class CurveDesc
{
    public string AnswerFeatureId;
    public AnswerFeature AnswerFeature;

    /// <summary>
    /// Question category, possible values: 
    /// - nobrainer, 
    /// - easy, 
    /// - middle
    /// - hard
    /// </summary>
    public string QuestionFeatureId;
    public QuestionFeature QuestionFeature;

    public string ColumnId { get { return ColumnLabel.Replace(" ", ""); } }

    public string ColumnLabel
    {
        get
        {
            if (AnswerFeature == null && QuestionFeature == null)
                return "Alle";

            var result = "";

            if (AnswerFeature != null && String.IsNullOrEmpty(AnswerFeature.Name))
                result = AnswerFeature.Name;

            if (QuestionFeature != null && String.IsNullOrEmpty(QuestionFeature.Name))
                result += " " + QuestionFeature.Name;

            return result;
        }
    }

    public ForgettingCurve LoadForgettingCurve(ForgettingCurveInterval interval, int maxIntervalCount)
    {
        return ForgettingCurveLoader.GetForAll(interval, maxIntervalCount);
    }
}