namespace TrueOrFalse.Core
{
    public class CreateQuestionValue_IfNotExists : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepository _questionValuationRepository;

        public CreateQuestionValue_IfNotExists(QuestionValuationRepository questionValuationRepository)
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
        }
    }
}
