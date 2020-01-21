

using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tools.Update.SetMigration
{
    public class SetMigrator
    {
        public static void Start()
        {
            var allSets = Sl.SetRepo.GetAll();

            foreach (var set in allSets)
            {

            }
        }

        private static IList<QuestionInSet> GetQuestions(Set set)
        {
            var questionList = new List<QuestionInSet>();
            var allQuestionInSet = Sl.QuestionInSetRepo.GetAll();


            return questionList;
        }
    }
}
