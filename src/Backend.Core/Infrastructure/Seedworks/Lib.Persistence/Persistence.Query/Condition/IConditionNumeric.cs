public interface IConditionNumeric
{
    string PropertyName { get; set; }
    object GetValue();

    bool IsGreaterThan();
    bool IsActive();
    bool IsSet();
    bool IsEqualTo();

}