using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;

public class SearchApiController : BaseController
{
    public JsonResult ByName(string term)
    {
        var items = new List<ResultItem>();
        var elements = SearchBoxElementsGet.Go(term);

        if (elements.Categories.Any())
        {
            AddHeader(items, ResultItemType.CategoriesHeader);
            AddCategoryItems(items, elements);
        }

        if (elements.Sets.Any())
        {
            AddHeader(items, ResultItemType.SetsHeader);
            AddSetItems(items, elements);
        }

        if (elements.Questions.Any())
        {
            AddHeader(items, ResultItemType.QuestionsHeader);
            AddQuestionItems(items, elements);
        }

        if (elements.Questions.Any())
        {
            AddHeader(items, ResultItemType.UsersHeader);
            AddUsersItems(items, elements);
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

    private static void AddCategoryItems(List<ResultItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Categories.Select(category => new ResultItem
            {
                ResultCount = elements.CategoriesResult.Count,
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

    private static void AddSetItems(List<ResultItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Sets.Select(set => new ResultItem
            {
                ResultCount = elements.SetsResult.Count,
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

    private static void AddQuestionItems(List<ResultItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Questions.Select(question => new ResultItem
            {
                ResultCount = elements.QuestionsResult.Count,
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

    private static void AddUsersItems(List<ResultItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Users.Select(user => new ResultItem
            {
                ResultCount = elements.UsersResult.Count,
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