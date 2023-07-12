using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class Send_knowledgeReport : BaseTest
{
    [Test][Ignore("Need to create TrainingDates etc for context user for test not to fail.")]
    public void ShouldSend()
    {
        var user = ContextUser.New(R<UserRepo>()).Add(new User { EmailAddress = "test@test.de", Name = "Firstname Lastname" }).Persist().All[0];
        var questions = ContextQuestion.New(R<QuestionRepo>(), R<AnswerRepo>(), R<AnswerQuestion>() )
            .AddQuestion(questionText: "q1", solutionText: "a1")
            .AddQuestion(questionText: "q2", solutionText: "a2")
            .AddQuestion(questionText: "q3", solutionText: "a3")
            .Persist();

        R<QuestionInKnowledge>().Create(new QuestionValuation { RelevancePersonal = 50, Question = questions.All[0], User = user });
        R<QuestionInKnowledge>().Create(new QuestionValuation { RelevancePersonal = 50, Question = questions.All[1], User = user });
        R<QuestionInKnowledge>().Create(new QuestionValuation { RelevancePersonal = 50, Question = questions.All[2], User = user });

        KnowledgeReportMsg.SendHtmlMail(user,
            R<JobQueueRepo>(),
            R<MessageEmailRepo>(), 
            R<GetAnswerStatsInPeriod>(), 
            R<GetStreaksDays>(), 
            R<UserRepo>(), 
            R<QuestionRepo>(), 
            R<GetUnreadMessageCount>(), 
            R<KnowledgeSummaryLoader>());
    }
} 