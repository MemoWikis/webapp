using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
    [Serializable]
    public enum Importance
	{
		None = 0,
		Prominent = 1
	}

	public interface ICategoryNumeric
	{
		object Value { get; }
	}

    [Serializable]
    public abstract class CategoryBaseAttribute : Attribute
	{
	}

    [Serializable]
    public class CategoryBooleanAttribute : CategoryBaseAttribute
    {
    }

    [Serializable]
    public class CategoryIntegerAttribute : CategoryBaseAttribute, ICategoryNumeric
    {
		public int Value { get; private set; }

		object ICategoryNumeric.Value
		{
			get { return Value; }
		}
		
		public CategoryIntegerAttribute(int value)
		{
			Value = value;
		}
    }

    [Serializable]
    public class CategorySingleAttribute : CategoryBaseAttribute, ICategoryNumeric
    {
		public Single Value { get; private set; }

		object ICategoryNumeric.Value
		{
			get { return Value; }
		}

		public CategorySingleAttribute(Single value)
		{
			Value = value;
		}
    }

    [Serializable]
    public class CategoryDoubleAttribute : CategoryBaseAttribute, ICategoryNumeric
    {
		public double Value { get; private set; }

		object ICategoryNumeric.Value
		{
			get { return Value; }
		}
		
		public CategoryDoubleAttribute(double value)
		{
			Value = value;
		}
    }
}
