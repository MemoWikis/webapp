using NHibernate;
using NHibernate.Criterion;


[Serializable]
public class ConditionObject<T> : Condition
{
    private T _value;

    public ConditionObject(ConditionContainer conditions) : base(conditions)
    {
    }

    public ConditionObject(ConditionContainer conditions, string propertyName) : base(conditions, propertyName)
    {
    }

    public void EqualTo(T value)
    {
        _value = value;
        AddUniqueToContainer();
    }

    public override void AddToCriteria(ICriteria criteria)
    {
        criteria.Add(GetCriterion());
    }

    public override ICriterion GetCriterion()
    {
        return Restrictions.Eq(PropertyName, _value);
    }
}