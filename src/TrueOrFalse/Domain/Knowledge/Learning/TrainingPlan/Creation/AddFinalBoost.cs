using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddFinalBoost
{
    public static void GetDatesForBoosting(AddFinalBoostParameters parameters)
    {
        
    }
}

public class AddFinalBoostParameters
{
    public List<DateTime> DatesForBoostingList;

    public List<int> NumberOfQuestionsPerSessionList;

    public int NumberOfBoostingDatesNeeded;

    public int unboostedQuestionsCount;

    public List<Question> AlreadyBoostedQuestions;

    public DateTime CurrentBoostingDateProposal;


}

