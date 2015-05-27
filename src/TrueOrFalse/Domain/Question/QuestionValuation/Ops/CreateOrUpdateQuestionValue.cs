public class CreateOrUpdateQuestionValue : IRegisterAsInstancePerLifetime
{
    private readonly QuestionValuationRepository _questionValuationRepository;

    public CreateOrUpdateQuestionValue(QuestionValuationRepository questionValuationRepository)
    {
        _questionValuationRepository = questionValuationRepository;
    }

    public void Run(int questionId, 
                    int userId, 
                    int quality = -2, 
                    int relevancePeronal = -2, 
                    int relevanceForAll = -2)
    {
        var questionValuation = _questionValuationRepository.GetBy(questionId, userId);

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = Sl.R<QuestionRepository>().GetById(questionId), 
                User = Sl.R<UserRepository>().GetById(userId), 
                Quality = quality, 
                RelevancePersonal = relevancePeronal, 
                RelevanceForAll = relevanceForAll
            };

            _questionValuationRepository.Create(newQuestionVal);
        }
        else
        {
            if (quality != -2) questionValuation.Quality = quality;
            if (relevancePeronal != -2) questionValuation.RelevancePersonal = relevancePeronal;
            if (relevanceForAll != -2) questionValuation.RelevanceForAll = relevanceForAll;

            _questionValuationRepository.Create(questionValuation);                
        }
        _questionValuationRepository.Flush();
    }
}