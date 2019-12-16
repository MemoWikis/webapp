using System.Collections.Generic;

public class QuestionListJson
{
    public int Page;
    public IList<Question> Questions;

    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CorrectnessProbability { get; set; }
        public string Answer { get; set; }
        public string ExtendedAnswer { get; set; }
        public string LinkToQuestion { get; set; }
        public ImageUrl ImageData { get; set; }
        public bool IsInWishknowledge { get; set; }
        public IList<SimpleList> CategoryList { get; set; }
        public QuestionAuthor Author { get; set; }
        public IList<SimpleList> SourceList { get; set; }


        public class SimpleList
        {
            public string Name = "";
            public string Url = "";
            public ImageUrl ImageUrl;
        }

        public class QuestionAuthor
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public ImageUrl ImageData { get; set; }
        }
    }
}




