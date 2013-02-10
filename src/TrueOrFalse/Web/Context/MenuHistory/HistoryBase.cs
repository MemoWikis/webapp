using System.Collections;
using System.Collections.Generic;

namespace TrueOrFalse
{
    public class HistoryBase<T> : IEnumerable<T> where T : HistoryItemBase
    {
        private readonly List<T> _list = new List<T>();

        public void Add(T historyItem)
        {
            _list.RemoveAll(x => x.Id == historyItem.Id && x.Type == historyItem.Type);
            _list.Insert(0, historyItem);

            while (_list.Count > 3)
                _list.RemoveAt(3);
        }

        public IEnumerator<T> GetEnumerator(){
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }
}
