using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class LearningTabModel :BaseModel
    {
        public Set set { get; set; }
        public TestSession testSession { get; set; }
        public SessionUser sessionUser { get; set; }
        public AnswerQuestionModel answerQuestionModel { get; set; }
        public Category coincidenceSet { get; set; }
        public bool isTestSession => !IsLoggedIn;
        public Category Category { get; set; }
         
       public LearningTabModel()
        {
            Category = Sl.CategoryRepo.Get
            if (isTestSession)
            {
                Random random = new Random();
                while (true)
                {
                    var allSets = Sl.CategoryRepo.GetAll();
                    var randomSet = random.Next(0, allSets.Count);
                    coincidenceSet = allSets[randomSet];
                    if (coincidenceSet.CountQuestions > 5)
                    {
                        break;
                    }
                }

                var randomQuestion = random.Next(0, coincidenceSet.CountQuestions);
                var dummyQuestion = Sl.QuestionRepo.GetById(randomQuestion);
                testSession = new TestSession(coincidenceSet);
                sessionUser = new SessionUser();
                sessionUser.AddTestSession(testSession);
                answerQuestionModel = new AnswerQuestionModel(testSession, Guid.NewGuid(), dummyQuestion, false);
            }
        }
    }
