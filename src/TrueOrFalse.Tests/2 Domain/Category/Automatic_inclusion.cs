using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;


class Automatic_inclusion : BaseTest
    {
        [Test]
        public void Test_Subcategory_add_correct_to_parent()
        {
            //ARANGE
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

            context.Update();

            Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub1").Id).ParentCategories().Count, Is.EqualTo(1)); 

            var c =   GraphService.GetAllParents(subCategories[0]);

          GraphService.AutomaticInclusionFromSubthemes(subCategories[0]);

        //ACT


        //ASSERT
    }
    }

