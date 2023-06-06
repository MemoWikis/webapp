using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UsageStatisticsOfLoggedInUsersResult
{
    public DateTime DateTime;

    public int QuestionsAnsweredCount;
    public int QuestionsViewedCount;
    public int LearningSessionsStartedCount;
    //public int TestSessionsStartedCount; // not stored
    public int DatesCreatedCount;

    public int UsersThatAnsweredQuestionCount;
    public int UsersThatViewedQuestionCount;
    public int UsersThatStartedLearningSessionCount;
    //public int UsersThatStartedTestSessionCount; // not stored
    public int UsersThatCreatedDateCount;
}
