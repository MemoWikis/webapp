public enum UserSettingNotificationInterval
{
    NotSet = 0,
    Never = 1,
    Daily = 2,
    Weekly = 3, //used as Standard-Value if NotSet, see UserSettings.aspx and KnowledgeReportMsg.ShouldSendToUser(xx)
    Monthly = 4,
    Quarterly = 5
}