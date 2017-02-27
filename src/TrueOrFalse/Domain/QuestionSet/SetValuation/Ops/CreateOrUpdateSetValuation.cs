public class CreateOrUpdateSetValuation 
{
    public static void Run(int setId,  int userId, int relevancePeronal = -2)
    {
        var setValuationRepo = Sl.SetValuationRepo;

        var setValuation = setValuationRepo.GetBy(setId, userId);

        if (setValuation == null){
            var newQuestionVal = 
                new SetValuation{ SetId = setId, UserId = userId, RelevancePersonal = relevancePeronal};

            setValuationRepo.Create(newQuestionVal);
        }
        else
        {
            if (relevancePeronal != -2) setValuation.RelevancePersonal = relevancePeronal;
            setValuationRepo.Update(setValuation);                
        }
        setValuationRepo.Flush();
    }
} 