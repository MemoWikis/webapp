namespace TrueOrFalse.Core
{
    public class CreateOrUpdateQuestionValue : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepository _questionValuationRepository;

        public CreateOrUpdateQuestionValue(QuestionValuationRepository questionValuationRepository)
        {
            _questionValuationRepository = questionValuationRepository;
        }

        public void Run(int questionId, 
                        int userId, 
                        int quality = -1, 
                        int relevancePeronal = -1, 
                        int relevanceForAll = -1)
        {
            var questionValuation = _questionValuationRepository.GetBy(questionId, userId);

            if (questionValuation == null)
            {
                var newQuestionVal = new QuestionValuation
                                         {
                                             QuestionId = questionId, 
                                             UserId = userId, 
                                             Quality = quality, 
                                             RelevancePersonal = relevancePeronal, 
                                             RelevanceForAll = relevanceForAll
                                         };

                _questionValuationRepository.Create(newQuestionVal);
            }
            else
            {
                if (quality != -1) questionValuation.Quality = quality;
                if (relevancePeronal != -1) questionValuation.RelevancePersonal = relevancePeronal;
                if (relevanceForAll != -1) questionValuation.RelevanceForAll = relevanceForAll;

                _questionValuationRepository.Create(questionValuation);                
            }
        }
    }
}
