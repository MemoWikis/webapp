using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class UpdateQuestionsOrder : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionInSetRepo _questionInSetRepo;

        public UpdateQuestionsOrder(QuestionInSetRepo questionInSetRepo){
            _questionInSetRepo = questionInSetRepo;
        }

        public void Run(IEnumerable<NewQuestionIndex> newIndicies)
        {
            foreach (var newQuestionIndex in newIndicies)
            {
                var questionInSet = _questionInSetRepo.Query.Where(
                    x => x.Id == newQuestionIndex.Id).SingleOrDefault();

                questionInSet.Sort = newQuestionIndex.NewIndex;
                _questionInSetRepo.Update(questionInSet);
            }
        }
    }
}
