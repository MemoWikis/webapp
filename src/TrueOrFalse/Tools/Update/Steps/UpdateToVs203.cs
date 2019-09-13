﻿using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs203
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
            @"ALTER TABLE `answer`
	                    ADD INDEX `IX_QuestionViewGuid` (`QuestionViewGuid`);"
                ).ExecuteUpdate();

            
        }
    }
}