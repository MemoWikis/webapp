using System;

public enum MainMenuEntry
{
    None, Knowledge, 
    Questions, QuestionDetail, 
    QuestionSet, QuestionSetDetail,
    Users, UserDetail,
    Categories, CategoryDetail,
    Help,
    Dates, DateDetail,
    About,
    Maintenance
}

[Serializable]
public class MainMenu
{
    public MainMenuEntry Current = MainMenuEntry.None;

    public string Active(MainMenuEntry mainMenuEntry)
    {
        if (Current == mainMenuEntry)
            return "active";

        return "";
    }
}