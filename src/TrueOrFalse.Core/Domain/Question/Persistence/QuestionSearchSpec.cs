using Seedworks.Lib.Persistence;
using TrueOrFalse.Core.Web.Context;

namespace TrueOrFalse.Core
{
    public class QuestionSearchSpec : SearchSpecificationBase<QuestionFilter, QuestionOrderBy>
    {
        public bool FilterByMe { get; private set; }
        public void SetFilterByMe(bool? value)
        {
            if (!value.HasValue) return;
            FilterByMe = value.Value;
            UpdateFilter();
        }

        public bool FilterByAll { get; private set; }
        public void SetFilterByAll(bool? value)
        {
            if (!value.HasValue) return;
            FilterByAll = value.Value;
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            if (FilterByMe && !FilterByAll)
            {
                Filter.CreatorId.EqualTo(ServiceLocator.Resolve<SessionUser>().User.Id);
            }
            else if (!FilterByMe && FilterByAll)
            {
                Filter.CreatorId.IsNotEqualTo(ServiceLocator.Resolve<SessionUser>().User.Id);
            }
            else
            {
                Filter.CreatorId.Remove();
            }
        }
    }

    public class QuestionFilter : ConditionContainer
    {
        public readonly ConditionInteger CreatorId;

        public QuestionFilter()
        {
            CreatorId = new ConditionInteger(this, "Creator.Id");
        }
    }

    public class QuestionOrderBy : SpecOrderByBase { }
}
