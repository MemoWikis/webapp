using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs050
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"CREATE TABLE IF NOT EXISTS `comment` (
                  `Id` int(11) NOT NULL AUTO_INCREMENT,
                  `Type` int(11) DEFAULT NULL,
                  `TypeId` int(11) DEFAULT NULL,
                  `Creator` longblob,
                  `Text` varchar(255) DEFAULT NULL,
                  `DateCreated` datetime DEFAULT NULL,
                  `DateModified` datetime DEFAULT NULL,
                  `AnswerTo` int(11) DEFAULT NULL,
                  PRIMARY KEY (`Id`),
                  KEY `AnswerTo` (`AnswerTo`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8;")
                .ExecuteUpdate();
        }
    }
}
