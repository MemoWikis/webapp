using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class HistoryBase<T> : IEnumerable<T> where T : HistoryItemBase
{
    protected int _size = 3;
    private readonly List<T> _list = new List<T>();

    public void Add(T historyItem)
    {
        if (historyItem == null)
            return;

        _list.RemoveAll(x => x.Id == historyItem.Id);
        _list.Insert(0, historyItem);

        while (_list.Count > _size)
            _list.RemoveAt(_size);
    }

    public IEnumerator<T> GetEnumerator(){
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator(){
        return GetEnumerator();
    }

    public string CssLast(int index)
    {
        return this.Count() == index ? " last" : "";
    }

    public string CssFirst(int index)
    {
        return index == 1 ? " first" : "";
    }
}