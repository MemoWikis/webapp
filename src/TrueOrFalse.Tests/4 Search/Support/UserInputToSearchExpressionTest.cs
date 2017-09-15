using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests
{
    public class UserInputToSearchExpressionTests 
    {
        [Test]
        public void Should_transform_userInput_to_valid_search_term()
        {
            Assert.That(InputToSearchExpression.Run(""), Is.EqualTo(""));
            Assert.That(InputToSearchExpression.Run("Juliane"), Is.EqualTo("(Juliane~)"));
            Assert.That(InputToSearchExpression.Run("\"Juliane\""), Is.EqualTo("(Juliane)"));
            Assert.That(InputToSearchExpression.Run("\"Juliane Misdom\""), Is.EqualTo("(Juliane Misdom)"));
            Assert.That(InputToSearchExpression.Run("\"Juliane Misdom\" \"Robert\""), Is.EqualTo("(Juliane Misdom Robert)"));
            Assert.That(InputToSearchExpression.Run("Juliane Misdom"), Is.EqualTo("(Juliane~ Misdom~)"));
            Assert.That(InputToSearchExpression.Run("   Juliane    Misdom   "), Is.EqualTo("(Juliane~ Misdom~)"));
            Assert.That(InputToSearchExpression.Run("Juliane Misdom \"Berlin Calling\""), Is.EqualTo("(Juliane~ Misdom~ Berlin Calling)"));
        }

        [Test]
        public void Should_build_search_expression()
        {
            var sqb = new SearchQueryBuilder()
                .Add("fieldName", "1")
                .Add("fieldName", "2")
                .Add("fieldName", "3");

            Assert.That(sqb.ToString(), Is.EqualTo("(fieldName:(1~) fieldName:(2~) fieldName:(3~))"));

            sqb = new SearchQueryBuilder()
                .Add("fieldName", "1")
                .Add("fieldName", "2", isAndCondition: true)
                .Add("fieldName", "3");

            Assert.That(sqb.ToString(), Is.EqualTo("(fieldName:(1~) fieldName:(3~)) AND fieldName:(2~)"));

            sqb = new SearchQueryBuilder()
                .Add("fieldName", "1", isAndCondition: true);

            Assert.That(sqb.ToString(), Is.EqualTo("fieldName:(1~)"));

            sqb = new SearchQueryBuilder()
                .Add("fieldName", "1")
                .Add("fieldName", "\"one two\"", startsWith: true);

            Assert.That(sqb.ToString(), Is.EqualTo("(fieldName:(1~) fieldName:(\"one two*\")^10)"));
        }
    }
}
