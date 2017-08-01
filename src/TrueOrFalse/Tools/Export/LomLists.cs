using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Provider;

public enum LomAggregationLevel
{
    Level1Fragment = 1,
    Level2Lesson = 2,
    Level3Course = 3,
    Level4Certificate = 4

}

public enum LomGeneralStructure
{
    Atomic = 1,
    Collection = 2,
    Networked = 3,
    Hierarchical = 4,
    Linear = 5
}

public enum LomLifecycleStatus
{
    Draft = 1,
    Final = 2,
    Revised = 3,
    Unavailable = 4
}

public enum LomLifecycleRole
{
    Author = 1,
    Publisher = 2,
    Unknown = 3,
    Initiator = 4,
    Terminator = 5,
    Validator = 6,
    Editor = 7,
    GraphicalDesigner = 8,
    TechnicalImplementer = 9,
    ContentProvider = 10,
    TechnicalValidator = 11,
    EducationalValidator = 12,
    ScriptWriter = 13,
    InstructionalDesigner = 14,
    SubjectMatterExpert = 15
}

public enum LomEducationalEndUser {
    Author = 1,
    Counsellor = 2,
    Learner = 3,
    Manager = 4,
    Parent = 5,
    Teacher = 6,
    Other = 7
}

public enum LomEducationalContext
{
    PreSchool = 1,
    CompulsoryEducation = 2,
    SpecialEducation = 3,
    VocationalEducation = 4,
    HigherEducation = 5,
    DistanceEducation = 6,
    ContinuingEducation = 7,
    ProfessionalDevelopment = 8,
    Library = 9,
    EducationalAdministration = 10,
    PolicyMaking = 11,
    Other = 12
}


static class LomListsExtensions
{
    public static int GetValue(this LomAggregationLevel aggregationLevel)
    {
        return (int)aggregationLevel;
    }

    public static  string GetValue(this LomLifecycleStatus lifecyclestatus)
    {
        return lifecyclestatus.ToString().ToLower();
    }

    public static string GetValue(this LomGeneralStructure structure)
    {
        return structure.ToString().ToLower();
    }

    public static string GetValue(this LomLifecycleRole role)
    {
        switch (role)
        {
            case LomLifecycleRole.Author: return "author";
            case LomLifecycleRole.Publisher: return "publisher";
            case LomLifecycleRole.Unknown: return "unknown";
            case LomLifecycleRole.Initiator: return "initiator";
            case LomLifecycleRole.Terminator: return "terminator";
            case LomLifecycleRole.Validator: return "validator";
            case LomLifecycleRole.Editor: return "editor";
            case LomLifecycleRole.GraphicalDesigner: return "graphical designer";
            case LomLifecycleRole.TechnicalImplementer: return "technical implementer";
            case LomLifecycleRole.ContentProvider: return "content provider";
            case LomLifecycleRole.TechnicalValidator: return "technical validator";
            case LomLifecycleRole.EducationalValidator: return "educational validator";
            case LomLifecycleRole.ScriptWriter: return "script writer";
            case LomLifecycleRole.InstructionalDesigner: return "instructional designer";
            case LomLifecycleRole.SubjectMatterExpert: return "subject matter expert";

            default: return "";
        }
    }

    public static string GetValue(this LomEducationalEndUser endUser)
    {
        return endUser.ToString().ToLower();
    }

    //public static string GetValue(this LomEducationalContext context)
    //{
    //    switch (context)
    //    {

    //        default: "other"
    //    }
    //}
}

