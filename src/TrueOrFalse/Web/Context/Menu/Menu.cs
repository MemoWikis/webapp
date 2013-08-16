using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public enum MenuEntry
    {
        None, Knowledge, 
        Questions, QuestionDetail, 
        QuestionSet, QuestionSetDetail,
        Users, UserDetail,
        Categories, CategoryDetail,
        News,
        Help
    }

    [Serializable]
    public class Menu
    {
        public MenuEntry Current = MenuEntry.None;

        public string Active(MenuEntry menuEntry)
        {
            if (Current == menuEntry)
                return "active";

            return "";
        }
    }
}
