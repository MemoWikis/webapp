using System;
using Newtonsoft.Json;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionHistoryDetailModel : BaseModel
{
    public int QuestionId;
    public string QuestionText;
    public string QuestionUrl;

    public bool PrevRevExists;
    public bool NextRevExists;

    public User Author;
    public string AuthorName;
    public string AuthorImageUrl;
    //public ImageFrontendData ImageFrontendData;
    
    public int RevisionId;
    public DateTime CurrentDateCreated;
    public string CurrentQuestionText;
    public string CurrentQuestionTextExtended;
    public string CurrentLicense;
    public string CurrentVisibility;
    public string CurrentSolution;
    public string CurrentSolutionDescription;
    public string CurrentSolutionMetadataJson;

    public DateTime PrevDateCreated;
    public string PrevQuestionText;
    public string PrevQuestionTextExtended;
    public string PrevLicense;
    public string PrevVisibility;
    public string PrevSolution;
    public string PrevSolutionDescription;
    public string PrevSolutionMetadataJson;

    public QuestionHistoryDetailModel(QuestionChange currentRevision, QuestionChange previousRevision,
        QuestionChange nextRevision)
    {
        var question = Sl.QuestionRepo.GetById(currentRevision.Question.Id);

        PrevRevExists = previousRevision != null;
        NextRevExists = nextRevision != null;

        var currentRevisionData = currentRevision.GetQuestionChangeData();
        RevisionId = currentRevision.Id;
        CurrentDateCreated = currentRevision.DateCreated;
        CurrentQuestionText = currentRevisionData.QuestionText;
        CurrentQuestionTextExtended = currentRevisionData.QuestionTextExtended?.Replace("\\r\\n", "\r\n");
        CurrentLicense = JsonConvert.SerializeObject(currentRevisionData.License);
        CurrentVisibility = currentRevisionData.Visibility.ToString();
        CurrentSolution = currentRevisionData.Solution;
        CurrentSolutionDescription = currentRevisionData.SolutionDescription?.Replace("\\r\\n", "\r\n");
        CurrentSolutionMetadataJson = currentRevisionData.SolutionMetadataJson;

        //if (currentRevision.DataVersion == 2)
        //{
        //    ImageWasUpdated = ((QuestionEditData_V2)currentRevisionData).ImageWasUpdated;
        //    var imageMetaData = Sl.ImageMetaDataRepo.GetBy(currentRevision.Question.Id, ImageType.Question);
        //    ImageFrontendData = new ImageFrontendData(imageMetaData);
        //}

        if (PrevRevExists)
        {
            var prevRevisionData = previousRevision.GetQuestionChangeData();
            PrevDateCreated = previousRevision.DateCreated;
            PrevQuestionText = prevRevisionData.QuestionText;
            PrevQuestionTextExtended = prevRevisionData.QuestionTextExtended?.Replace("\\r\\n", "\r\n");
            PrevLicense = JsonConvert.SerializeObject(prevRevisionData.License);
            PrevVisibility = prevRevisionData.Visibility.ToString();
            PrevSolution = prevRevisionData.Solution;
            PrevSolutionDescription = prevRevisionData.SolutionDescription?.Replace("\\r\\n", "\r\n");
            PrevSolutionMetadataJson = prevRevisionData.SolutionMetadataJson;

            //if (currentRevision.DataVersion >= 2 && previousRevision.DataVersion >= 2)
            //{
            //    var currentRelationsList = ((QuestionEditData_V2)currentRevisionData).QuestionRelations;
            //    var prevRelationsList = ((QuestionEditData_V2)prevRevisionData).QuestionRelations;

            //    CurrentRelations = SortedListOfRelations(currentRelationsList);
            //    PrevRelations = SortedListOfRelations(prevRelationsList);
            //}
        }

        QuestionId = currentRevision.Question.Id;
        QuestionUrl = Links.AnswerQuestion(question);
        QuestionText = currentRevision.Question.Text;
        Author = currentRevision.Author;
        AuthorName = currentRevision.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentRevision.Author.Id).GetUrl_85px_square(currentRevision.Author).Url;
    }


    //private string Relation2String(QuestionRelation_EditData relation)
    //{
    //    var relatedQuestion = Sl.QuestionRepo.GetById(relation.RelatedQuestionId);
    //    string res;
    //    switch (relation.RelationType)
    //    {
    //        case QuestionRelationType.IsChildQuestionOf:
    //            res = $"\"{relatedQuestion.Name}\" (ist übergeordnet)";
    //            break;
    //        case QuestionRelationType.IncludesContentOf:
    //            res = $"\"{relatedQuestion.Name}\" (ist untergeordnet)";
    //            break;
    //        default:
    //            res = $"\"{relatedQuestion.Name}\" (hat undefinierte Beziehung)";
    //            break;
    //    }

    //    return res;
    //}

    //private string SortedListOfRelations(IList<QuestionRelation_EditData_V2> relations)
    //{
    //    string res = "";
    //    if (relations.IsNotEmpty())
    //    {
    //        var parents = relations.Where(r => r.RelationType == QuestionRelationType.IsChildQuestionOf);
    //        res += "Übergeordnete Fragen\n";
    //        res += (parents.IsEmpty())
    //            ? "<keine>"
    //            : string.Join("\n", parents.Select(Relation2String));

    //        var children = relations.Where(r => r.RelationType == QuestionRelationType.IncludesContentOf);
    //        res += "\n\nUntergeordnete Fragen\n";
    //        res += (children.IsEmpty())
    //            ? "<keine>"
    //            : string.Join("\n", children.Select(Relation2String));

    //        var otherRelations = relations.Where(r => r.RelationType != QuestionRelationType.IsChildQuestionOf && r.RelationType != QuestionRelationType.IncludesContentOf);
    //        res += "\n\nAndere Beziehungsdaten\n";
    //        res += (otherRelations.IsEmpty())
    //            ? "<keine>"
    //            : string.Join("\n", children.Select(Relation2String));
    //    }

    //    return res;
    //}
}

