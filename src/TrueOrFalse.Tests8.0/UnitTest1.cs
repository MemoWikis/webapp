using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;


namespace TrueOrFalse.Tests8._0
{
    public class Tests : BaseTest
    {
        public Tests()
        {
        }

  

        [Test]
        public void Test1()
        {
            ContextCategory.New().Add("A").Persist(); 

        }
    }
}