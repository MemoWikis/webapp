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
        public static string KeyPagerAll = "all";
        public static string KeyPagerMine = "mine";
        public static string KeyPagerWish = "wish";

        private readonly SessionUiData _sessionUiData;

        public QuestionSearchSpecSession(SessionUiData sessionUiData){
            _sessionUiData = sessionUiData;
        }

        public QuestionSearchSpec ByKey(string key)
        {
            if (_sessionUiData.SearchSpecQuestions.Any(x => x.Key == key))
                return _sessionUiData.SearchSpecQuestions.First(x => x.Key == key);

            QuestionSearchSpec activeSearchSpec;                
            if (key == "mine")
                activeSearchSpec = _sessionUiData.SearchSpecQuestionMine;
            else if (key == "wish")
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

        public static string GetKey(bool isTabAll, bool isTabMine, bool isTabWish)
        {
            if (isTabAll) return KeyPagerAll;
            if (isTabMine) return KeyPagerMine;
            if (isTabWish) return KeyPagerWish;

            throw new Exception("invalid combination");
        }

        public static string GetUrl(string key)
        {
            if (key == KeyPagerMine) return Links.QuestionsMine();
            if (key == KeyPagerWish) return Links.QuestionsWish();
            return Links.QuestionsAll();
        }
    }
}