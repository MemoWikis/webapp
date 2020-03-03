using System;
using System.Text;

namespace TemplateMigration
{

    public class Part
    {
        public PartType Type;

        private StringBuilder _sb = new StringBuilder();

        public bool IsTemplate => Type == PartType.Template;
        public bool IsText => Type == PartType.Text;

        public void AddChar(char character) => _sb.Append(character);
        public string ToText() => _sb.ToString();
        public void AddNewLine() => _sb.Append(Environment.NewLine);

        public bool Contains(string searchString) => ToText().ToLower().Trim().Contains(searchString);

        public bool IsTopicNavigation => Contains("templatename") && Contains("topicnavigation");
        public bool IsCategoryNetwork => Contains("templatename") && Contains("categorynetwork");
    }

}