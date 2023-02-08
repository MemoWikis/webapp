using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch
{
    internal class MeiliSearchCategoriesDatabaseOperationsTests : MeiliSearchBaseTests
    {
        [Test(Description = "Set TestCategory in MeiliSearch")]
        public async Task CreateCategoryTest()
        {
            //construction
            await DeleteCategories();
            var category = new Category
            {
                Id = 12,
                Name = "Daniel",
                Creator = new User
                {
                    Id = 15
                },
                Description = "Description",
                DateCreated = DateTime.MaxValue,
                CountQuestions = 2000
            };

            //Execution
            await new MeiliSearchCategoriesDatabaseOperations()
                .CreateAsync(category, CategoriesTest)
                .ConfigureAwait(false);

            var index = client.Index(CategoriesTest);
            var result = (await index.SearchAsync<MeiliSearchCategoryMap>(category.Name).ConfigureAwait(false)).Hits.ToList();
            var categoryMap = result.First();

            //Tests 
            Assert.AreEqual(result.GetType(), typeof(List<MeiliSearchCategoryMap>));
            Assert.True(result.Count == 1);

            Assert.AreEqual(categoryMap.Name, category.Name);
            Assert.AreEqual(DateTime.MaxValue, categoryMap.DateCreated);
            Assert.AreEqual(categoryMap.Id, category.Id);
            Assert.AreEqual(categoryMap.Description, category.Description);
            Assert.AreEqual(categoryMap.QuestionCount, category.CountQuestions);
            Assert.AreEqual(categoryMap.CreatorId, category.Creator.Id);
        }

        [Test(Description = "Update TestCategoryr in MeiliSearch")]
        public async Task UpdateUserTest()
        {
            //construction
            await DeleteCategories();
            var category = new Category
            {
                Id = 12,
                Name = "Daniel",
                Creator = new User
                {
                    Id = 15
                },
                Description = "Description",
                DateCreated = DateTime.MaxValue,
                CountQuestions = 2000
            };

            //Execution
            await new MeiliSearchCategoriesDatabaseOperations()
                .CreateAsync(category, CategoriesTest)
                .ConfigureAwait(false);

            category.Name = "Daniela";
            await new MeiliSearchCategoriesDatabaseOperations()
                .UpdateAsync(category, CategoriesTest)
                .ConfigureAwait(false);

            var index = client.Index(CategoriesTest);
            var result = (await index.SearchAsync<MeiliSearchCategoryMap>(category.Name).ConfigureAwait(false)).Hits.ToList();
            var categoryMap = result.First();

            //Tests 
            Assert.AreEqual(result.GetType(), typeof(List<MeiliSearchCategoryMap>));
            Assert.True(result.Count == 1);

            Assert.AreEqual(category.Name, categoryMap.Name );
            Assert.AreEqual(DateTime.MaxValue, categoryMap.DateCreated);
            Assert.AreEqual(categoryMap.Id, category.Id);
            Assert.AreEqual(categoryMap.Description, category.Description);
            Assert.AreEqual(categoryMap.QuestionCount, category.CountQuestions);
            Assert.AreEqual(categoryMap.CreatorId, category.Creator.Id);
        }


        [Test(Description = "Delete TestCategoryin MeiliSearch")]
        public async Task DeleteUserTest()
        {
            //construction
            await DeleteCategories();
            var category = new Category
            {
                Id = 12,
                Name = "Daniel",
                Creator = new User
                {
                    Id = 15
                },
                Description = "Description",
                DateCreated = DateTime.MaxValue,
                CountQuestions = 2000
            };

            //Execution
            await new MeiliSearchCategoriesDatabaseOperations()
                .CreateAsync(category, CategoriesTest)
                .ConfigureAwait(false);

            category.Name = "Daniela";
            await new MeiliSearchCategoriesDatabaseOperations()
                .DeleteAsync(category, CategoriesTest)
                .ConfigureAwait(false);

            var index = client.Index(CategoriesTest);
            var result = (await index.SearchAsync<MeiliSearchCategoryMap>(category.Name).ConfigureAwait(false)).Hits.ToList();
            var categoryMap = result.FirstOrDefault();

            //Tests 
            Assert.True(result.IsNullOrEmpty());
            Assert.True(categoryMap == null);
        }
    }
}
