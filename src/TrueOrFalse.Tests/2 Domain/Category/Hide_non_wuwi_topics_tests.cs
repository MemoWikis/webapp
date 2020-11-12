using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TrueOrFalse.Tests._2_Domain.Category
{
    class Hide_non_wuwi_topics_tests: BaseTest
    {

        [Test]
        public void Add_should_category_history()
        {
            var context = ContextCategory.New();
            var rootElement = context.Add("RootElement").Persist().All.First();
            var firstChildren = context.Add("Sub1", parent: rootElement).Add("Sub2", parent: rootElement).Persist().All.ByName("Sub1");
            var secondChildren = context.Add("SubSub1", parent: firstChildren).Persist().All.ByName("SubSub1");
            var thirdChildren = context.Add("SubSubSub1", parent: secondChildren).Persist().All.ByName("SubSubSub1"); 
        }
    }
}
