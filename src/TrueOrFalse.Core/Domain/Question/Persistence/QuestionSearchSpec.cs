using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class QuestionSearchSpec : SearchSpecificationBase<QuestionFilter, QuestionOrderBy> { }

    public class QuestionFilter : ConditionContainer
    {
        public readonly ConditionInteger CreatorId;

        public QuestionFilter()
        {
            CreatorId = new ConditionInteger(this, "CreatorId");
        }
    }

    public class QuestionOrderBy : SpecOrderByBase { }
}
