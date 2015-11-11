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
                    int relevancePersonal = -2, 
                    int relevanceForAll = -2)
    {
        var questionValuation = _questionValuationRepo.GetBy(questionId, userId);

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = Sl.R<QuestionRepo>().GetById(questionId), 
                User = Sl.R<UserRepo>().GetById(userId), 
                Quality = quality, 
                RelevancePersonal = relevancePersonal, 
                RelevanceForAll = relevanceForAll
            };

            _questionValuationRepo.Create(newQuestionVal);
        }
        else
        {
            if (quality != -2) questionValuation.Quality = quality;
            if (relevancePersonal != -2) questionValuation.RelevancePersonal = relevancePersonal;
            if (relevanceForAll != -2) questionValuation.RelevanceForAll = relevanceForAll;

            _questionValuationRepo.Update(questionValuation);                
        }
        _questionValuationRepo.Flush();
    }
}