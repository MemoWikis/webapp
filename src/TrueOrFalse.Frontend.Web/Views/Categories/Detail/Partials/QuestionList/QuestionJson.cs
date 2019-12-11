using System.Collections.Generic;

public class QuestionListJson
{
    public int Page;
    public IList<Question> Questions;

    public class Question
    {
        public int Id;
        public string Title;
        public string LearningStatus;
        public string Answer = "";
        public string ExtendedAnswer = "";
        public string LinkToQuestion = "";
        public IList<SimpleList> CategoryList;
        public IList<SimpleList> AuthorList;
        public IList<SimpleList> SourceList;


        public class SimpleList
        {
            public string Name = "";
            public string Url = "";
            public ImageUrl ImageUrl;
        }
    }
}




