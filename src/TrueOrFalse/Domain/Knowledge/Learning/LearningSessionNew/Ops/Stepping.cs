using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Domain.Knowledge.Learning.LearningSessionNew.Ops
{
    class Stepping
    {
        private List<LearningSessionStepNew> LearningSessionSteps;

      public Stepping( List<LearningSessionStepNew> learningSessionSteps)
        {
            LearningSessionSteps = learningSessionSteps;
        }
      
        private  void SetAnswerLearningSessionStepAnswerNew(int questionId,bool isAnswerCorrect)
        {
            LearningSessionSteps.ForEach(lss => { if (lss.Question.Id == questionId) lss.Answered = isAnswerCorrect; }); 
        }

    }
}
