using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using TrueOrFalse;

public class EditQuestionModel_to_Question : IRegisterAsInstancePerLifetime
{
    public Question Create(EditQuestionModel model, NameValueCollection postData)
    {
        var question = new Question();
        return Update(model, question, postData);
    }

    public Question Update(EditQuestionModel model, Question question, NameValueCollection postData)
    {
        question.Text = model.Question;
        question.TextExtended = model.QuestionExtended;

        question.Description = model.Description;
        question.Categories.Clear();

        foreach (var category in model.Categories)
            AddCategory(question, category);

        question.Solution = model.Solution;
        question.SolutionType = (SolutionType) Enum.Parse(typeof(SolutionType), model.SolutionType);

        question.Visibility = model.Visibility;

        var serializer = new JavaScriptSerializer();
        switch (question.SolutionType)
        {
            case SolutionType.Text:
                var solutionModel = new QuestionSolutionExact();
                solutionModel.FillFromPostData(postData);
                question.Solution = solutionModel.Text;
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

    private void AddCategory(Question question, string categoryName)
    {
        var category = ServiceLocator.Resolve<CategoryRepository>().GetByName(categoryName);

        if (category == null)
            throw new InvalidDataException();

        question.Categories.Add(category);
    }
}