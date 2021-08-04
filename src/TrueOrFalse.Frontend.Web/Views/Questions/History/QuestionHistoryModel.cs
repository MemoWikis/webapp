using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Util;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionHistoryModel : BaseModel
{
    private class SiblingData
    {
        public bool NewerRevisionExists;
        public bool OlderRevisionExists;
        public int OlderRevisionId;
    }

    public int QuestionId;
    public string QuestionText;
    public string QuestionUrl;
    public IList<QuestionChangeDayModel> Days;
    
    public QuestionHistoryModel(Question question, IList<QuestionChange> revisions)
    {
        QuestionText = question.Text;
        QuestionId = question.Id;
        QuestionUrl = Links.AnswerQuestion(question);

        Days = revisions
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new QuestionChangeDayModel(
                group.Key,
                (IList<QuestionChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();

        // Gather data on the older and newer sibling of a revision
        revisions = revisions.OrderByDescending(qc => qc.DateCreated).ToList();
        var siblings = new Dictionary<int, SiblingData>();
        for (int i = 0; i < revisions.Count; i++)
        {
            // Careful: the following is only correct if the revisions object contains all revisions
            siblings.Add(revisions[i].Id, new SiblingData()
            {
                NewerRevisionExists = (i > 0),
                OlderRevisionExists = (i < revisions.Count - 1),
                OlderRevisionId = (i < revisions.Count - 1) ? revisions[i + 1].Id : 0
            });
        }

        // Create and populate dictionary rev id => rev model
        var id2RevModelDict = new Dictionary<int, QuestionChangeDetailModel>();
        Days.ForEach(day => 
            day.RevisionModels.ForEach(revisionModel =>
                id2RevModelDict.Add(revisionModel.RevisionId, revisionModel)
            ));
        
        // Assign old sibling to each rev model and load image data
        Days.ForEach(day => 
            day.RevisionModels.ForEach(revModel =>
            {
                var revId = revModel.RevisionId;
                var initialRevId = revisions[revisions.Count - 1].Id;
                revModel.NewerRevisionExists = siblings[revId].NewerRevisionExists;
                revModel.OlderRevisionExists = siblings[revId].OlderRevisionExists;

                if (revId == initialRevId)
                    // The initial revision doesn't have an older sibling so we create one with null values
                    revModel.OlderRevision = new QuestionChangeDetailModel() {};
                else
                    revModel.OlderRevision = id2RevModelDict[siblings[revId].OlderRevisionId];

                if (!revModel.OlderRevisionExists || revModel.ImageWasChanged)
                {
                    var imageMetaData = Sl.ImageMetaDataRepo.GetBy(revModel.QuestionId, ImageType.Question);
                    revModel.ImageFrontendData = new ImageFrontendData(imageMetaData);
                }
            }
        ));
    }
}

public class QuestionChangeDayModel
{
    public string Date;
    public IList<QuestionChangeDetailModel> RevisionModels;

    public QuestionChangeDayModel(DateTime date, IList<QuestionChange> revisions)
    {
        Date = date.ToString("dd.MM.yyyy");
        RevisionModels = revisions.Select(rev =>
        {
            var data = rev.GetQuestionChangeData();
            var revisionModel = new QuestionChangeDetailModel
            {
                Author = rev.Author,
                AuthorName = rev.Author.Name,
                AuthorImageUrl = new UserImageSettings(rev.Author.Id).GetUrl_85px_square(rev.Author).Url,
                ElapsedTime = TimeElapsedAsText.Run(rev.DateCreated),
                DateTime = rev.DateCreated.ToString("dd.MM.yyyy HH:mm"),
                Time = rev.DateCreated.ToString("HH:mm"),
                RevisionId = rev.Id,
                QuestionId = rev.Question.Id,
                QuestionName = rev.Question.Text,
                DateCreated = rev.DateCreated,
                QuestionText = data.QuestionText,
                QuestionTextExtended = data.QuestionTextExtended?.Replace("\\r\\n", "\r\n"),
                License = JsonConvert.SerializeObject(data.License),
                Visibility = data.Visibility.ToString(),
                Solution = data.Solution,
                SolutionDescription = data.SolutionDescription?.Replace("\\r\\n", "\r\n"),
                SolutionMetadataJson = data.SolutionMetadataJson,
                ImageWasChanged = data.ImageWasChanged
            };
            return revisionModel;
        }).ToList();
    }
}

public class QuestionChangeDetailModel
{
    public User Author;
    public string AuthorName;
    public string AuthorImageUrl;

    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int RevisionId;
    public int QuestionId;
    public string QuestionName;
    
    public bool ImageWasChanged;
    public ImageFrontendData ImageFrontendData;
    
    public QuestionChangeDetailModel OlderRevision;
    public bool NewerRevisionExists;
    public bool OlderRevisionExists;

    public DateTime DateCreated;
    public string QuestionText;
    public string QuestionTextExtended;
    public string License;
    public string Visibility;
    public string Solution;
    public string SolutionDescription;
    public string SolutionMetadataJson;
}
