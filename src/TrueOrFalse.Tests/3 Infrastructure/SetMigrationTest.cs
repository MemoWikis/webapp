using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
class SetMigrationTest : BaseTest
{
    [Test]
    public void Should_select_questions_with_matching_setId()
    {
        ContextSet.GetPersistedSampleSet();

        RecycleContainer();
        var setFromDB = R<SetRepo>().GetAll();

        var categoryRepo = Resolve<CategoryRepository>();

        var user = new User { Name = "Some user" };
        Resolve<UserRepo>().Create(user);

        var setValuation1 = new SetValuation { SetId = 1, UserId = 1, RelevancePersonal = 7 };
        var setValuation2 = new SetValuation { SetId = 1, UserId = 2, RelevancePersonal = 7 };

        Resolve<SetValuationRepo>().Create(new List<SetValuation> { setValuation1, setValuation2 });
        var setValuationFromDB = R<SetValuationRepo>().GetAll();

        var questionRepo = Resolve<QuestionRepo>();
        var questionsFromDB = questionRepo.GetAll();


        foreach (var set in setFromDB)
        {

            var category = new Category()
            {
                Name = set.Name,
                Type = CategoryType.Standard,
                Creator = set.Creator,
                DateCreated = set.DateCreated,
                FormerSetId = set.Id,
            };

            categoryRepo.Create(category);
            var newId = category.Id;

            foreach (var relatedCategory in set.Categories)
                ModifyRelationsForCategory.AddParentCategory(category, relatedCategory);

            var setValuationList = setValuationFromDB.Where(sV => sV.SetId == set.Id).ToList();

            foreach (var question in set.QuestionsInSet)
                question.Question.Categories.Add(category);

            foreach (var setValuation in setValuationList)
                CreateOrUpdateCategoryValuation.Run(category.Id, setValuation.UserId, setValuation.RelevancePersonal);
        }

        var questionsFromDB2 = questionRepo.GetAll();
        var categoriesFromDb = categoryRepo.GetAll();

        var categoryFromDb = categoryRepo.GetAll().First();
        var categoryValuationFromDB = R<CategoryValuationRepo>().GetAll();

        RecycleContainer();
    }
}

