using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs041
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"CREATE TABLE IF NOT EXISTS `message` (
                  `Id` int(11) NOT NULL AUTO_INCREMENT,
                  `ReceiverId` int(11) DEFAULT NULL,
                  `Subject` varchar(255) DEFAULT NULL,
                  `Body` varchar(255) DEFAULT NULL,
                  `MessageType` varchar(255) DEFAULT NULL,
                  `IsRead` tinyint(1) DEFAULT NULL,
                  `DateCreated` datetime DEFAULT NULL,
                  `DateModified` datetime DEFAULT NULL,
                  PRIMARY KEY (`Id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8;").ExecuteUpdate();
        }
    }
}
