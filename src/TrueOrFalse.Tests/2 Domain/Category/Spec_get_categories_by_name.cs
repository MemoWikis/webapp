using System;
using System.Collections.Generic;
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

        }

    }
}
