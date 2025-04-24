using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch
{
    internal class GlobalSearchResultTest
    {
        [Test(Description = "Member Categories Tests")]
        public void CategoriesMembersTests()
        {
            var categoriesResult = A.Fake<ISearchCategoriesResult>();
            A.CallTo(() => categoriesResult.GetCategories()).Returns(new List<CategoryCacheItem>
            {
                new(){Name = "Daniel"},
                new(){Name = "Majunke"}
            });
            A.CallTo(() => categoriesResult.Count).Returns(3); 

            var globalSearchResult = new GlobalSearchResult
            {
                CategoriesResult = categoriesResult,
            }; 

            Assert.AreEqual(globalSearchResult.Categories.Count, 2);
            Assert.AreEqual(globalSearchResult.Categories.First().Name, "Daniel");
            Assert.AreEqual(globalSearchResult.Categories.Last().Name, "Majunke");
            Assert.AreEqual(globalSearchResult.CategoriesResultCount, 3);
        }

        [Test(Description = "Member Questions Tests")]
        public void QUestionsMemberTests()
        {
            var questionsResult = A.Fake<ISearchQuestionsResult>();
            A.CallTo(() => questionsResult.GetQuestions()).Returns(new List<QuestionCacheItem>
            {
                new(){Text = "Daniel"},
                new(){Text = "Majunke"}
            });
            A.CallTo(() => questionsResult.Count).Returns(3);

            var globalSearchResult = new GlobalSearchResult
            {
                QuestionsResult = questionsResult,
            };

            Assert.AreEqual(globalSearchResult.Questions.Count, 2);
            Assert.AreEqual(globalSearchResult.Questions.First().Text, "Daniel");
            Assert.AreEqual(globalSearchResult.Questions.Last().Text, "Majunke");
            Assert.AreEqual(globalSearchResult.QuestionsResultCount, 3);
        }

        [Test(Description = "Member Users Tests")]
        public void UsersMemberTests()
        {
            var usersResult = A.Fake<ISearchUsersResult>();
            A.CallTo(() => usersResult.GetUsers()).Returns(new List<UserCacheItem>
            {
                new(){Name = "Daniel"},
                new(){Name = "Majunke"}
            });
            A.CallTo(() => usersResult.Count).Returns(3);

            var globalSearchResult = new GlobalSearchResult
            {
                UsersResult = usersResult,
            };

            Assert.AreEqual(globalSearchResult.Users.Count, 2);
            Assert.AreEqual(globalSearchResult.Users.First().Name, "Daniel");
            Assert.AreEqual(globalSearchResult.Users.Last().Name, "Majunke");
            Assert.AreEqual(globalSearchResult.UsersResultCount, 3);
        }

        [Test(Description = "Member Users Tests")]
        public void TotalMemberTests()
        {
            var categoriesResult = A.Fake<ISearchCategoriesResult>();
            A.CallTo(() => categoriesResult.GetCategories()).Returns(new List<CategoryCacheItem>
            {
                new(){Name = "Daniel"},
                new(){Name = "Majunke"}
            });
            var questionsResult = A.Fake<ISearchQuestionsResult>();
            A.CallTo(() => questionsResult.GetQuestions()).Returns(new List<QuestionCacheItem>
            {
                new(),
                new()
            });
            var usersResult = A.Fake<ISearchUsersResult>();
            A.CallTo(() => usersResult.GetUsers()).Returns(new List<UserCacheItem>
            {
                new(),
                new()
            });

            var globalSearchResult = new GlobalSearchResult
            {
                CategoriesResult = categoriesResult,
                QuestionsResult = questionsResult,
                UsersResult = usersResult
            };

            Assert.AreEqual(globalSearchResult.TotalElements, 6);
          
        }
    }
}
