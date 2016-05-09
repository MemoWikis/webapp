using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class AutocompleteUtils
{
    public static IList<Category> GetRelatedCategoriesFromPostData(NameValueCollection postData)
    {
        var _categoryRepo = ServiceLocator.Resolve<CategoryRepository>();
        return
            (from key in postData.AllKeys
                where key.StartsWith("cat-")
                select _categoryRepo.GetById(Convert.ToInt32(postData[key])))
                .Where(category => category != null)
                .ToList();
    }

    public static IList<Set> GetSetsFromPostData(NameValueCollection postData)
    {
        var _setRepo = ServiceLocator.Resolve<SetRepo>();
        return
            (from key in postData.AllKeys
                where key.StartsWith("set-")
                select _setRepo.GetById(Convert.ToInt32(postData[key])))
                .Where(category => category != null)
                .ToList();
    }

    public static int GetQuestionsCountFromPostData(NameValueCollection postData)
    {
        var sets = GetSetsFromPostData(postData);
        var questionCount = 0;
        foreach (var set in sets)
        {
            questionCount += set.Questions().Count;
        }
        return questionCount;
    }
}