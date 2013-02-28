using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs020
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery(@"
                    CREATE TABLE `questioninset` (
                      `Id` int(11) NOT NULL AUTO_INCREMENT,
                      `DateCreated` datetime DEFAULT NULL,
                      `DateModified` datetime DEFAULT NULL,
                      `Question_id` int(11) DEFAULT NULL,
                      `QuestionSet_id` int(11) DEFAULT NULL,
                      PRIMARY KEY (`Id`),
                      KEY `Question_id` (`Question_id`),
                      KEY `QuestionSet_id` (`QuestionSet_id`),
                      CONSTRAINT `FKD34040F3A925A27` FOREIGN KEY (`QuestionSet_id`) REFERENCES `questionset` (`Id`),
                      CONSTRAINT `FKD34040F20054BCB` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`)
                    ) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8")
                .ExecuteUpdate();
        }
    }
}
