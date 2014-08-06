using System;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Utils;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
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
            if (key == SearchTab.Mine.ToString())
                activeSearchSpec = _sessionUiData.SearchSpecQuestionMine;
            else if (key == SearchTab.Wish.ToString())
                activeSearchSpec = _sessionUiData.SearchSpecQuestionWish;
            else
                activeSearchSpec = _sessionUiData.SearchSpecQuestionAll;

            activeSearchSpec.KeyOverviewPage = key;

            return CloneAndAddToSession(activeSearchSpec);
        }

        public static QuestionSearchSpec CloneAndAddToSession(QuestionSearchSpec searchSpec, QuestionHistoryItem historyItem = null)
        {
            var result = searchSpec.DeepClone();
            result.Key = Guid.NewGuid().ToString();

            if (historyItem != null)
                result.HistoryItem = historyItem.DeepClone();

            Sl.Resolve<SessionUiData>().SearchSpecQuestions.Add(result);
            return result;
        }

        public static string GetUrl(SearchTab searchTab)
        {
            if (searchTab == SearchTab.Mine) return Links.QuestionsMine();
            if (searchTab == SearchTab.Wish) return Links.QuestionsWish();
            return Links.QuestionsAll();
        }
    }
}