using System;
using System.Collections.Generic;
using System.Web;
using TrueOrFalse.Web;

public class EditDateModel : BaseModel
{
    public bool IsEditing;
    public UIMessage Message;

    public int DateId { get; set; }
    
    public string Details { get; set; }

    public DateTime Date { get; set; }
    public string Time { get; set; }

    public IList<Set> Sets { get; set; }

    public string Visibility { get; set; }

    public EditDateModel()
    {
        Date = DateTime.Now.Date.AddDays(3);
        Time = "19:00";
        Sets = new List<Set>();
    }

    public EditDateModel(Date date)
    {
        DateId = date.Id;
        IsEditing = true;
        Details = date.Details;
        Date = date.DateTime;
        Time = new Time(date.DateTime).ToString();
        Sets = date.Sets;

        if(date.Visibility == DateVisibility.Private)
            Visibility = "private";
        else if(date.Visibility == DateVisibility.InNetwork)
            Visibility = "inNetwork";
        else
            throw new Exception("invalid visibility");
    }

    public string Selected(string visibilty)
    {
        if (visibilty == Visibility)
            return "selected=\"selected\"";

        return "";
    }

    public Date ToDate()
    {
        return FillDateFromInput(new Date());
    }

    public Date FillDateFromInput(Date date)
    {
        date.Sets = AutocompleteUtils.GetReleatedSetsFromPostData(HttpContext.Current.Request.Form);
        date.Details = Details;
        date.DateTime = global::Time.Parse(Time).SetTime(Date);
        date.User = _sessionUser.User;

        if (Visibility == "inNetwork")
            date.Visibility = DateVisibility.InNetwork;
        else if (Visibility == "private")
            date.Visibility = DateVisibility.Private;
        else
            throw new Exception("Invalid mapping");

        return date;
    }

    public bool IsDateTimeInPast()
    {
        return DateTime.Now > global::Time.Parse(Time).SetTime(Date);
    }

    public bool HasSets()
    {
        return AutocompleteUtils.GetReleatedSetsFromPostData(HttpContext.Current.Request.Form).Count > 0;
    }

    public bool HasErrorMsg()
    {
        if (Message == null)
            return false;

        return Message.IsErrorMessage();
    }
}