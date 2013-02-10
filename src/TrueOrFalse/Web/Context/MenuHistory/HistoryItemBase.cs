using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public enum HistoryItemType{ Any, Edit}

    public interface HistoryItemBase
    {
        int Id { get;}
        HistoryItemType Type { get; }
    }
}
