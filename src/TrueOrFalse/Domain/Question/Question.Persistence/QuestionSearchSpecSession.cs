
public class QuestionSearchSpecSession : IRegisterAsInstancePerLifetime
{
    public static QuestionSearchSpec AddCloneToSession(QuestionSearchSpec searchSpec, QuestionHistoryItem historyItem = null)
    {
        var result = searchSpec.DeepClone();
        result.Key = Guid.NewGuid().ToString();
         
        if (historyItem != null)
            result.HistoryItem = historyItem.DeepClone();

        Sl.SessionUiData.SearchSpecQuestions.Add(result);
        return result;
    }
}