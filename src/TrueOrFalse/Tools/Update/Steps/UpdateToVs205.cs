using TrueOrFalse;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs205
    { 
        public static void Run()
        {
            var questions = Sl.QuestionRepo.GetAll();
            foreach (var question in questions)
            {
                var questionsChanges = Sl.QuestionChangeRepo.GetForQuestion(question.Id);
                bool isQuestionAvailable ()=> questionsChanges.Count > 0;

                if(isQuestionAvailable())
                    Sl.QuestionChangeRepo.UpdateWithoutSession(question, questionsChanges[0].Id);
                else
                {
                    Sl.QuestionChangeRepo.CreateQuestionChange(question);
                    questionsChanges = Sl.QuestionChangeRepo.GetForQuestion(question.Id);
                    Sl.QuestionChangeRepo.UpdateWithoutSession(question, questionsChanges[0].Id);
                }
            }
        }
    }
}