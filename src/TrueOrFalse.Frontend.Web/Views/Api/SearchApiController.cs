using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

public class SearchApiController : BaseController
{
    public JsonResult ByName(string term)
    {
        var items = new List<ResultItem>();

        var categories = Sl.SearchCategories.Run(term, new Pager { PageSize = 5 }, searchOnlyWithStartingWith: true).GetCategories();
        var sets = Sl.SearchSets.Run(term, new Pager { PageSize = 5 }, searchOnlyWithStartingWith: true).GetSets();
        var questions = Sl.SearchQuestions.Run(term, new Pager { PageSize = 5 }).GetQuestions();
        var users = Sl.SearchUsers.Run(term, new Pager { PageSize = 5 }, SearchUsersOrderBy.None).GetUsers();

        if (categories.Any())
        {
            AddHeader(items, ResultItemType.SetsHeader);
            AddCategoryItems(items, categories);
        }

        if (sets.Any())
        {
            AddHeader(items, ResultItemType.CategoriesHeader);
            AddSetItems(items, sets);
        }

        if (questions.Any())
        {
            AddHeader(items, ResultItemType.QuestionsHeader);
            AddQuestionItems(items, questions);
        }

        if (questions.Any())
        {
            AddHeader(items, ResultItemType.UsersHeader);
            AddUsersItems(items, users);
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

    private static void AddCategoryItems(List<ResultItem> items, IList<Category> categories)
    {
        items.AddRange(
            categories.Select(category => new ResultItem
            {
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

    private static void AddSetItems(List<ResultItem> items, IList<Set> sets)
    {
        items.AddRange(
            sets.Select(set => new ResultItem
            {
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

    private static void AddQuestionItems(List<ResultItem> items, IList<Question> questions)
    {
        items.AddRange(
            questions.Select(question => new ResultItem
            {
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

    private static void AddUsersItems(List<ResultItem> items, IList<User> users)
    {
        items.AddRange(
            users.Select(user => new ResultItem
            {
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