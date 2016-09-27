using System;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using TrueOrFalse;

public class EditQuestionModel_to_Question 
{
    public static Question Create(EditQuestionModel model, NameValueCollection postData)
    {
        var question = new Question();
        return Update(model, question, postData);
    }

    public static Question Update(EditQuestionModel model, Question question, NameValueCollection postData)
    {
        question.Text = model.Question;
        question.TextExtended = model.QuestionExtended;

        question.Description = model.Description;
        question.Categories = model.Categories;

        question.SolutionType = (SolutionType) Enum.Parse(typeof(SolutionType), model.SolutionType);

        question.UpdateReferences(model.References);

        question.Visibility = model.Visibility;
        question.IsWorkInProgress = false;

        question.License = Sl.R<SessionUser>().IsInstallationAdmin 
            ? LicenseQuestionRepo.GetById(model.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();

        var serializer = new JavaScriptSerializer();
        switch (question.SolutionType)
        {
            case SolutionType.Text:
                var solutionModel = new QuestionSolutionExact();
                solutionModel.FillFromPostData(postData);
                question.Solution = solutionModel.Text.Trim();
                question.SolutionMetadataJson = solutionModel.MetadataSolutionJson;
                break;

            case SolutionType.Sequence:
                var solutionModel1 = new QuestionSolutionSequence();
                solutionModel1.FillFromPostData(postData);
                question.Solution = serializer.Serialize(solutionModel1);
                break;

            case SolutionType.MultipleChoice:
                var solutionModel2 = new QuestionSolutionMultipleChoice();
                solutionModel2.FillFromPostData(postData);
                question.Solution = serializer.Serialize(solutionModel2);
                break;
        }

        return question;
    }

}