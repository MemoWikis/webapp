using System.Collections.Generic;

class SetComparerIdEquals : IEqualityComparer<Set>
{
    public bool Equals(Set x, Set y)
    {
        return x.Id == y.Id;
    }

    public int GetHashCode(Set obj)
    {
        return obj.GetHashCode();
    }
}