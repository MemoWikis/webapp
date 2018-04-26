using System;
using System.Linq;

public class LearningTabModel : BaseModel
{
    public TestSession TestSession { get; set; }
    public int TestSessionId { get; set; } 
    public SessionUser SessionUser { get; set; }
    public AnswerQuestionModel AnswerQuestionModel { get; set; }
    public bool IsTestSession => !IsLoggedIn;
         
    public LearningTabModel(Category category)
    {
            
        if (IsTestSession)
        {
            TestSession = new TestSession(category);
            SessionUser = new SessionUser();
            SessionUser.AddTestSession(TestSession);
            TestSessionId = TestSession.Id; 

            var step = TestSession.Steps.ToList().First();
            AnswerQuestionModel = new AnswerQuestionModel(TestSession, Guid.NewGuid(), Sl.QuestionRepo.GetById(step.QuestionId), false);
        } else
        {
            TestSessionId = -1;
        }


        //TestSession = Sl.SessionUser.TestSessions.Find(t => t.Id == testSessionId);
        //var step = TestSession.Steps.ToList().First();
        //AnswerQuestionModel = new AnswerQuestionModel(TestSession, Guid.NewGuid(), Sl.QuestionRepo.GetById(step.QuestionId), false);
    }
}
