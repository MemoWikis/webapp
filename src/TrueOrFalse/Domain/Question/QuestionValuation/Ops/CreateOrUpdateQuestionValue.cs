public class CreateOrUpdateQuestionValue : IRegisterAsInstancePerLifetime
{
    private readonly QuestionValuationRepo _questionValuationRepo;

    public CreateOrUpdateQuestionValue(QuestionValuationRepo questionValuationRepo)
    {
        _questionValuationRepo = questionValuationRepo;
    }

    public void Run(int questionId, 
                    int userId, 
                    int quality = -2, 
                    int relevancePeronal = -2, 
                    int relevanceForAll = -2)
    {
        var questionValuation = _questionValuationRepo.GetBy(questionId, userId);

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = Sl.R<QuestionRepository>().GetById(questionId), 
                User = Sl.R<UserRepo>().GetById(userId), 
                Quality = quality, 
                RelevancePersonal = relevancePeronal, 
                RelevanceForAll = relevanceForAll
            };

            _questionValuationRepo.Create(newQuestionVal);
        }
        else
        {
            if (quality != -2) questionValuation.Quality = quality;
            if (relevancePeronal != -2) questionValuation.RelevancePersonal = relevancePeronal;
            if (relevanceForAll != -2) questionValuation.RelevanceForAll = relevanceForAll;

            _questionValuationRepo.Create(questionValuation);                
        }
        _questionValuationRepo.Flush();
    }
}