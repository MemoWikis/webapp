using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public class UserNavigationModelList : IEnumerable<UserNavigationModel>
    {
        private readonly List<UserNavigationModel> _list = new List<UserNavigationModel>();

        public void Add(UserNavigationModel userNavigationModel)
        {
            _list.RemoveAll(x => x.Id == userNavigationModel.Id);
            _list.Insert(0, userNavigationModel);

            while(_list.Count > 3)
                _list.RemoveAt(3);
        }

        public IEnumerator<UserNavigationModel> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
