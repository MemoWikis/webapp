using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
class SetMigrationTest
{
    [Test]
    public void Should_select_questions_with_matching_setId()
    {
        var questionList = new List<QuestionInSet>();
        var allQuestionInSet = Sl.QuestionInSetRepo.GetAll();
        var test = "test";
    }
}

