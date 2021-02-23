using System.Linq;
using NUnit.Framework;
using SolrNet.Commands.Cores;
using TrueOrFalse.Tests;


class Automatic_inclusion_tests : BaseTest
    {
        [Test]
        public void Test_Subcategory_add_correct_to_parent()
        { 
            var context = ContextCategory.New(); 
            var parentA = context
                .Add("Category")
                .Persist()
                .All.First();

            var subCategories = ContextCategory
                .New()
                .Add("Sub1", parent: parentA)
                .Add("Sub2", parent: parentA)
                .Add("Sub3", parent: parentA)
                .Persist()
                .All;

            context.Add(subCategories[0].Name, subCategories[0].Type, parent: subCategories.ByName("Sub3"));
            EntityCache.Init();
            GraphService.AutomaticInclusionOfChildThemes(subCategories.ByName("Sub1"));

            Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub1").Id).ParentCategories().Count, Is.EqualTo(2));
            Assert.That(Sl.CategoryRepo.GetById(parentA.Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(3));
            Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub3").Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(1));

        }
    }

