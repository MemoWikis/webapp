using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seedworks.Lib;
using TrueOrFalse;

public class QuestionSetRowModel
{
    public int Id;
    public string Name;
    public string DescriptionShort;

    public int IndexInResult;

    public int QuestionCount;

    public string CreatorName;
    public bool IsOwner;

    public QuestionSetRowModel(QuestionSet questionSet, int indexInResultSet, int currentUserid)
    {
        Id = questionSet.Id;
        Name = questionSet.Name;
        DescriptionShort = questionSet.Text.Wrap(150);

        CreatorName = questionSet.Creator.Name;
        IsOwner = currentUserid == questionSet.Creator.Id;
        IndexInResult = indexInResultSet;
    }

}