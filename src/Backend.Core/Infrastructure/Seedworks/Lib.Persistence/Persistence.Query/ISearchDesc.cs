﻿public interface ISearchDesc : IPager
{
    ConditionContainer Filter { get; }
    OrderByCriteria OrderBy { get; }
}