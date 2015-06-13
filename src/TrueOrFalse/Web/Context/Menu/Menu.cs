using System;

public enum MenuEntry
{
    None, Knowledge, 
    Questions, QuestionDetail, 
    QuestionSet, QuestionSetDetail,
    Users, UserDetail,
    Categories, CategoryDetail,
    Messages,
    Help,
    Dates,
    Play
}

[Serializable]
public class Menu
{
    public MenuEntry Current = MenuEntry.None;

    public string Active(MenuEntry menuEntry)
    {
        if (Current == menuEntry)
            return "active";

        return "";
    }
}