using System.Collections.Generic;

public class Stepping
    {
        public List<LearningSessionStepNew> LearningSessionSteps;

      public Stepping( List<LearningSessionStepNew> learningSessionSteps)
        {
            LearningSessionSteps = learningSessionSteps;
        }
      
        public  void SetAnswerLearningSessionStepAnswerNew(int questionId,bool isAnswerCorrect, bool isAnswered, bool isSkip)
        {
            for(var i = 0; i < LearningSessionSteps.Count; i++)
            {
                if (LearningSessionSteps[i].Question.Id == questionId)
                {
                    LearningSessionSteps[i].IsAnswerCorrect = isAnswerCorrect;
                    LearningSessionSteps[i].IsAnswered = isAnswered;
                    LearningSessionSteps[i].IsSkip = isSkip;

                    if (isAnswered && !isAnswerCorrect || isSkip)
                    {
                        LearningSessionSteps.Add( new LearningSessionStepNew(LearningSessionSteps[i].Question) {IsAnswered = false, IsAnswerCorrect = false, IsSkip = false});
                        break;
                    }
                }
            }; 
        }
    }

