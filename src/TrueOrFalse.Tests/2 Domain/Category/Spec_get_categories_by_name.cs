using System;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class Spec_get_categories_by_name : BaseTest
    {
        [Test]
        public void Should_retrieve_category_by_name()
        {
            ContextCategory.New().
                Add("Cat1").
                Add("Cat2").
                Add("Cat3").Persist();

            Assert.That(Resolve<CategorySearch>().Run("CAT").Count, Is.EqualTo(3));
            Assert.That(Resolve<CategorySearch>().Run("cat").Count, Is.EqualTo(3));
            Assert.That(Resolve<CategorySearch>().Run("3").Count, Is.EqualTo(1));
            Assert.That(Resolve<CategorySearch>().Run("at2").Count, Is.EqualTo(1));
        }

    }
}
