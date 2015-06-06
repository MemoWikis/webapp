using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs062
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();
            session.CreateSQLQuery(@"
CREATE TABLE IF NOT EXISTS `reference` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AdditionalInfo` varchar(255) DEFAULT NULL,
  `FreeTextReference` varchar(255) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `DateModified` datetime DEFAULT NULL,
  `Question_id` int(11) DEFAULT NULL,
  `Category_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Question_id` (`Question_id`),
  KEY `Category_id` (`Category_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
").ExecuteUpdate();
        }
    }
}