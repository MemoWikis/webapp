using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Search;

public class SearchApiController : BaseController
{
    public JsonResult ByName(string term)
    {
        var items = new List<ResultItem>();


        var elements = SearchBoxElementsGet.Go(term);

        if (elements.Categories.Any())
        {
            AddHeader(items, ResultItemType.SetsHeader);
            AddCategoryItems(items, elements.CategoriesResult);
        }

        if (elements.Sets.Any())
        {
            AddHeader(items, ResultItemType.CategoriesHeader);
            AddSetItems(items, elements.SetsResult);
        }

        if (elements.Questions.Any())
        {
            AddHeader(items, ResultItemType.QuestionsHeader);
            AddQuestionItems(items, elements.QuestionsResult);
        }

        if (elements.Questions.Any())
        {
            AddHeader(items, ResultItemType.UsersHeader);
            AddUsersItems(items, elements.UsersResult);
        }
        
        return Json( new{ Items = items }, JsonRequestBehavior.AllowGet);
    }

    private static void AddHeader(List<ResultItem> items, ResultItemType resultItemType)
    {
        items.Add(new ResultItem
        {
            Type = resultItemType.ToString(),
            Item = new ResultSplitter
            {
                Text = "Some Text"
            }
        });
    }

    private static void AddCategoryItems(List<ResultItem> items, SearchCategoriesResult categoriesResult)
    {
        items.AddRange(
            categoriesResult.GetCategories().Select(category => new ResultItem
            {
                ResultCount = categoriesResult.Count,
                Type = ResultItemType.Categories.ToString(),
                Item = new ResultItemJson
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = new CategoryImageSettings(category.Id).GetUrl_50px(asSquare:true).Url,
                }
            })
        );
    }

    private static void AddSetItems(List<ResultItem> items, SearchSetsResult setsResult)
    {
        items.AddRange(
            setsResult.GetSets().Select(set => new ResultItem
            {
                ResultCount = setsResult.Count,
                Type = ResultItemType.Sets.ToString(),
                Item = new ResultItemJson
                {
                    Id = set.Id,
                    Name = set.Name,
                    ImageUrl = new SetImageSettings(set.Id).GetUrl_50px_square().Url,
                }
            })
        );
    }

    private static void AddQuestionItems(List<ResultItem> items, SearchQuestionsResult questionsResult)
    {
        items.AddRange(
            questionsResult.GetQuestions().Select(question => new ResultItem
            {
                ResultCount = questionsResult.Count,
                Type = ResultItemType.Questions.ToString(),
                Item = new ResultItemJson
                {
                    Id = question.Id,
                    Name = question.Text.Wrap(200),
                    ImageUrl = new QuestionImageSettings(question.Id).GetUrl_50px_square().Url
                }
            })
        );
    }

    private static void AddUsersItems(List<ResultItem> items, SearchUsersResult usersResult)
    {
        items.AddRange(
            usersResult.GetUsers().Select(user => new ResultItem
            {
                ResultCount = usersResult.Count,
                Type = ResultItemType.Users.ToString(),
                Item = new ResultItemJson
                {
                    Id = user.Id,
                    Name = user.Name,
                    ImageUrl = new UserImageSettings(user.Id).GetUrl_50px_square(user).Url
                }
            })
        );
    }

    private enum ResultItemType
    {
        CategoriesHeader,
        Categories,
        SetsHeader,
        Sets,
        QuestionsHeader,
        Questions,
        UsersHeader,
        Users,
    }

    private class ResultItem
    {
        public int ResultCount;
        public string Type;
        public object Item;
    }

    private class ResultSplitter
    {
        public string Text;
    }

    public class ResultItemJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}