using System.Linq;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class Should_calculate_wish_knowledge_count : BaseTest
    {
        [Test]
        public void Run()
        {
            var contextQuestion = ContextQuestion.New()
                .AddQuestion(questionText: "QuestionA", solutionText: "AnswerA").AddCategory("A")
                .Persist();

            var question = contextQuestion.All.First();
            var user = contextQuestion.Creator;
            var updateTotals = Resolve<UpdateQuestionTotals>();

            updateTotals.Run(new QuestionValuation { RelevancePersonal = 100, Question = question, User = user });
            updateTotals.Run(new QuestionValuation { RelevancePersonal = 1, Question = question, User = user });
            updateTotals.Run(new QuestionValuation { Question = question, User = user });

            Assert.That(Resolve<GetWishQuestionCount>().Run(userId:2), Is.EqualTo(2));
        }
    }
}
