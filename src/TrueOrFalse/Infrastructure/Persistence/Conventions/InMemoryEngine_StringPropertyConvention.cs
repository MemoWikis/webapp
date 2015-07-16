using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

public class InMemoryEngine_StringPropertyConvention : IPropertyConvention
{
    public void Apply(IPropertyInstance instance)
    {
        if (instance.Property.PropertyType == typeof(string))
            instance.CustomSqlType("VARCHAR(1000)");
    }
}