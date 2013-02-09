using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public enum MenuEntry
    {
        None, Knowledge, Questions, QuestionSet, Categories, News, Network
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
