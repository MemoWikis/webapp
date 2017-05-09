using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs173
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `relatedcategoriestorelatedcategories`
                    ADD COLUMN `Id` INT(10) NOT NULL AUTO_INCREMENT FIRST,
	                ADD PRIMARY KEY (`Id`),
	                ADD COLUMN `CategoryRelationType` INT(10) NULL DEFAULT '1' AFTER `Related_Id`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                @"ALTER TABLE `relatedcategoriestorelatedcategories`
                	CHANGE COLUMN `CategoryRelationType` `CategoryRelationType` INT(10) NULL DEFAULT NULL AFTER `Related_Id`;"
                ).ExecuteUpdate();
        }
    }
}