using System;
using System.Linq;
using FluentNHibernate.Utils;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionSearchSpecSession : IRegisterAsInstancePerLifetime
{
    private readonly SessionUiData _sessionUiData;

    public QuestionSearchSpecSession(SessionUiData sessionUiData){
        _sessionUiData = sessionUiData;
    }

    public QuestionSearchSpec ByKey(string key)
    {
        if (_sessionUiData.SearchSpecQuestions.Any(x => x.Key == key))
            return _sessionUiData.SearchSpecQuestions.First(x => x.Key == key);

        QuestionSearchSpec activeSearchSpec;
        if (key == SearchTabType.Mine.ToString())
        {
            activeSearchSpec = _sessionUiData.SearchSpecQuestionMine;
            activeSearchSpec.SearchTab = SearchTabType.Mine;                                
        }
        else if (key == SearchTabType.Wish.ToString())
        {
            activeSearchSpec = _sessionUiData.SearchSpecQuestionWish;
            activeSearchSpec.SearchTab = SearchTabType.Wish;
        }
        else if (key == "searchbox")
        {
            activeSearchSpec = _sessionUiData.SearchSpecQuestionSearchBox;
            activeSearchSpec.SearchTab = SearchTabType.All;
        }
        else
        {
            activeSearchSpec = _sessionUiData.SearchSpecQuestionAll;
            activeSearchSpec.SearchTab = SearchTabType.All;
        }

        return AddCloneToSession(activeSearchSpec);
    }

    public static QuestionSearchSpec AddCloneToSession(QuestionSearchSpec searchSpec, QuestionHistoryItem historyItem = null)
    {
        var result = searchSpec.DeepClone();
        result.Key = Guid.NewGuid().ToString();
         
        if (historyItem != null)
            result.HistoryItem = historyItem.DeepClone();

        Sl.SessionUiData.SearchSpecQuestions.Add(result);
        return result;
    }

    public static string GetUrl(SearchTabType searchTab)
    {
        if (searchTab == SearchTabType.Mine) return Links.QuestionsMine();
        if (searchTab == SearchTabType.Wish) return Links.QuestionsWish();
        return Links.QuestionsAll();
    }
}