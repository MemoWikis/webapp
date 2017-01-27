class UpdateSetting
{
    public static void RunForKnowledgeReportInterval(User user, UserSettingNotificationInterval newKnowledgeReportInterval)
    {
        user.KnowledgeReportInterval = newKnowledgeReportInterval;
        Sl.R<UserRepo>().Update(user);
    }
}
