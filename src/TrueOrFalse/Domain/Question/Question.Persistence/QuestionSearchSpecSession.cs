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

            QuestionSearchSpec activeSearchSpec = null;                
            if (key == "mine")
                activeSearchSpec = _sessionUiData.SearchSpecQuestionMine;
            else if (key == "wish")
                activeSearchSpec = _sessionUiData.SearchSpecQuestionWish;
            else
                activeSearchSpec = _sessionUiData.SearchSpecQuestionAll;

            var result = activeSearchSpec.DeepClone();
            result.Key = Guid.NewGuid().ToString();
            result.KeyOverviewPage = key;
            _sessionUiData.SearchSpecQuestions.Add(result);
            return result;
        }

        public static string GetKey(bool isTabAll, bool isTabMine, bool isTabWish)
        {
            if (isTabAll) return KeyPagerAll;
            if (isTabMine) return KeyPagerMine;
            if (isTabWish) return KeyPagerWish;

            throw new Exception("invalid combination");
        }

        public static string GetUrl(UrlHelper urlHelper, string key)
        {
            if (key == KeyPagerMine) return Links.QuestionsMine(urlHelper);
            if (key == KeyPagerWish) return Links.QuestionsWish(urlHelper);
            return Links.QuestionsAll(urlHelper);
        }
    }
}