using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ObjectDumper;
using Remotion.Linq.Clauses.ResultOperators;

namespace TrueOrFalse.Tests
{
    public class GetRandomLogoCssClassTest : BaseTest
    {
        [Test]
        public void Should_get_random_logo_css_class()
        {
            //Arrange + //Act
            var results = new List<string>();
            for (var i = 0; i < 100; i++)
                results.Add(GetRandomLogoCssClass.Run(DateTime.Now.AddDays(i)));

            //Assert
            foreach (var cssClass in GetRandomLogoCssClass.CssClasses)
                Assert.That(results.Count(c => c == cssClass),
                    Is.GreaterThan((results.Count / GetRandomLogoCssClass.CssClasses.Count) - 10), 
                    "CssClass:" + cssClass);

        }
    }
}
