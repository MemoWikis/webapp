using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
    public interface IDataService<T>
    {
        void Create(T item);

        void Update(T item);

        void Delete(T item);
        void Delete(int id);

        IList<T> GetAll();
        T GetById(int id);
        IList<T> GetBy(ISearchDesc searchDesc);
    }
}
