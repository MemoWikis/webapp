using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class SpecFilterBase : ConditionContainer
    {
        public readonly ConditionDateTime DateCreated;
        public readonly ConditionDateTime DateModified;

        public SpecFilterBase()
        {
            DateCreated = new ConditionDateTime(this, "DateCreated");
            DateModified = new ConditionDateTime(this, "DateModified");
        }
    }
}
