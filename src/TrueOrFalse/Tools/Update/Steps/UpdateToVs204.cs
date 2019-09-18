using System;
using System.Linq;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs204
    {
        public static void Run()
        {
            var allUsers = Sl.UserRepo.GetAll();
            var repoPosition = allUsers.OrderByDescending(x=> x.ReputationPos).FirstOrDefault().ReputationPos + 1;
                               

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
            @"INSERT INTO User(`PasswordHashedAndSalted`, `Salt`, `EmailAddress`, `Name`, `IsEmailConfirmed`, `IsInstallationAdmin`, `AllowsSupportiveLogin`, `ShowWishKnowledge`,
                `KnowledgeReportInterval`, `Reputation`, `WishCountQuestions`, `WishCountSets`, `CorrectnessProbability`,
                `CorrectnessProbabilityAnswerCount`, `ActivityPoints`, `ActivityLevel`, `DateCreated`, `DateModified`,`ReputationPos` )
            VALUES('395dc0200dac66a04faba12040a0ab97', '82f3d587-ddba-48d6-808c-7f97174f613c',
                    'team@memucho.de', 'unbekannt', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '18-06-12 10:34:09', '18-06-12 10:34:09', "+ repoPosition +");"
                ).ExecuteUpdate();


        }
    }
}