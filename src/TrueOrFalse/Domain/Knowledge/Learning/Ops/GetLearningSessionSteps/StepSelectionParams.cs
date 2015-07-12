using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StepSelectionParams
{
    public IList<Question> AllQuestions;
    public IEnumerable<Question> UnansweredQuestions;
    public IEnumerable<Question> AnsweredQuestions;
    public IEnumerable<Question> QuestionsAnsweredToday;

    public IList<TotalPerUser> AllTotals;
    public IList<QuestionValuation> AllValuations;
    public IList<AnswerHistory> AllAnswerHistories; 

    //public bool AllHaveBeenAnsweredToday;
}
