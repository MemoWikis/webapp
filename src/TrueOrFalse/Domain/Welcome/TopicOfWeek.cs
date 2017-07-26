using System;
using System.Collections.Generic;

public class TopicOfWeek
{
    public DateTime FirstValidDay;
    public int CategoryId;
    public string TopicOfWeekTitle;
    public string TopicDescriptionHTML;

    public int QuizOfWeekSetId;
    //public IList<int> AdditionalSetsIds;
    public IList<int> AdditionalCategoriesIds;

}
