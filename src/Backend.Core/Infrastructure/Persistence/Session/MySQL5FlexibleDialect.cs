using NHibernate.Dialect;

public class MySQL5FlexibleDialect : MySQL5Dialect
{
    public static bool IsInMemoryEngine()
    {
        return Engine == "MEMORY";
    }

    public static string Engine = "InnoDB";
}