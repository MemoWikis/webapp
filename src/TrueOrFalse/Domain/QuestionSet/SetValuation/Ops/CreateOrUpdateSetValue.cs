public class CreateOrUpdateSetValue : IRegisterAsInstancePerLifetime
{
    private readonly SetValuationRepository _setValuationRepo;

    public CreateOrUpdateSetValue(SetValuationRepository setValuationRepo){
        _setValuationRepo = setValuationRepo;
    }

    public void Run(int setId,  int userId, int relevancePeronal = -2)
    {
        var setValuation = _setValuationRepo.GetBy(setId, userId);

        if (setValuation == null){
            var newQuestionVal = 
                new SetValuation{ SetId = setId, UserId = userId, RelevancePersonal = relevancePeronal};

            _setValuationRepo.Create(newQuestionVal);
        }
        else
        {
            if (relevancePeronal != -2) setValuation.RelevancePersonal = relevancePeronal;
            _setValuationRepo.Create(setValuation);                
        }
        _setValuationRepo.Flush();
    }
}