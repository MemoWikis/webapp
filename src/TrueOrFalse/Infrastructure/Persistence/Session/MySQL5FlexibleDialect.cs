using NHibernate.Dialect;

namespace TrueOrFalse
{
    public class MySQL5FlexibleDialect : MySQL5Dialect
    {
        public static string Engine = "InnoDB";

        public override string TableTypeString
        {
            get { return " ENGINE=" + MySQL5FlexibleDialect.Engine; }
        }

        public override bool HasSelfReferentialForeignKeyBug
        {
            get { return true; }
        }

        public override bool SupportsCascadeDelete
        {
            get { return true; }
        }
    }
}