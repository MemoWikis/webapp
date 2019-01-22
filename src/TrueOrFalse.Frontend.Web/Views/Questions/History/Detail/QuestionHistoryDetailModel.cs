using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
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

    public int RevisionId;
    public bool ImageWasChanged;
    public ImageFrontendData ImageFrontendData;

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
        }
        
        if (!PrevRevExists || currentRevisionData.ImageWasChanged)
        {
            ImageWasChanged = true;
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(currentRevision.Question.Id, ImageType.Question);
            ImageFrontendData = new ImageFrontendData(imageMetaData);
        }

        QuestionId = currentRevision.Question.Id;
        QuestionUrl = Links.AnswerQuestion(question);
        QuestionText = currentRevision.Question.Text;
        Author = currentRevision.Author;
        AuthorName = currentRevision.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentRevision.Author.Id).GetUrl_85px_square(currentRevision.Author).Url;
    }

}

