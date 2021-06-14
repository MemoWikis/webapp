using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

namespace TemplateMigration
{
    public static class QuestionSolutionType3To7Migration
    {
        public static void Run()
        {
            var allQuestions = Sl.QuestionRepo.GetAll();
            foreach (var question in allQuestions)
            {
                if (question.SolutionType != SolutionType.MultipleChoice_SingleSolution)
                    continue;

                dynamic oldSolution = JsonConvert.DeserializeObject(question.Solution);
                var newSolution = new Solution
                {
                    isSolutionOrdered = false,
                    Choices = new List<Choice>(),
                };
                int index = 0;
                foreach (string choice in oldSolution.Choices)
                {
                    var isCorrect = false || index == 0;
                    var newChoice = new Choice
                    {
                        Text = choice, 
                        IsCorrect = isCorrect
                    };
                    newSolution.Choices.Add(newChoice);
                    index++;
                }

                question.Solution = JsonConvert.SerializeObject(newSolution);
                question.SolutionType = SolutionType.MultipleChoice;

                Sl.QuestionRepo.UpdateBeforeEntityCacheInit(question);
            }
        }

        public class Solution
        {
            public List<Choice> Choices;
            public bool isSolutionOrdered;
        }

        public class Choice
        {
            public string Text;
            public bool IsCorrect;
        }
    }
}