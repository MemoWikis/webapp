using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs065
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `imagemetadata`
	                ADD COLUMN `MainLicense` INT(11) NULL DEFAULT NULL AFTER `Markup`,
	                ADD COLUMN `ApplicableLicenses` VARCHAR(1000) NULL DEFAULT NULL AFTER `MainLicense`,
	                ADD COLUMN `OtherLicenses` VARCHAR(1000) NULL DEFAULT NULL AFTER `ApplicableLicenses`,
	                ADD COLUMN `LicenseStatus` INT(11) NULL DEFAULT NULL AFTER `OtherLicenses`")
                 .ExecuteUpdate();
            //'OtherLicenses' für erfasste Licenses/ID oder geparste mögliche License strings?
        } 
    }
}
