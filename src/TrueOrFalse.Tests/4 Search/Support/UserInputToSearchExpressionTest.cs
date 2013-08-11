using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Assert.That(InputToSearchExpression.Run("Juliane Misdom"), Is.EqualTo("(Juliane~ Misdom~)"));
            Assert.That(InputToSearchExpression.Run("   Juliane    Misdom   "), Is.EqualTo("(Juliane~ Misdom~)"));
            Assert.That(InputToSearchExpression.Run("Juliane Misdom \"Berlin Calling\""), Is.EqualTo("(Juliane~ Misdom~ Berlin Calling)"));

            
        }
    }
}
