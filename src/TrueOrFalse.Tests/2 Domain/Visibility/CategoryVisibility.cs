using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests._2_Domain.Visibility
{
    class CategoryVisibility : BaseTest
    {
        [Test]
        public void ImpenetrablePrivateCategories()
        {
            //Arrange
            var context = ContextCategory.New();
            var parentA = context
                .Add("Category")
                .Persist()
                .All.First();

            var subCategories = ContextCategory
                .New()
                .Add("Sub1")
                .Add("Sub2", parent: parentA)
                .Add("Sub3", parent: parentA)
                .Persist()
                .All;

            context
                .Add(subCategories.ByName("Sub1").Name, parent: subCategories.ByName("Sub3"))
                .Persist();
            
            EntityCache.Init();

            subCategories.ByName("Sub3").Visibility = global::CategoryVisibility.Owner;

            ContextQuestion.New().AddQuestions(15, 
                categoriesQuestions: subCategories.Where(sc => sc.Name == "Sub1").ToList(), 
                persistImmediately: true);
            
            EntityCache.Init();

            Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub1").Id).GetCountQuestionsAggregated(), Is.EqualTo(15));
            Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub3").Id).GetCountQuestionsAggregated(), Is.EqualTo(15));
            Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Category").Id).GetCountQuestionsAggregated(), Is.EqualTo(0));
        }
    }
}
