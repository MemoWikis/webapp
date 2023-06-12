using System.Collections.Generic;
using System.Linq;

namespace Seedworks.Lib.Persistence
{
    public abstract class BaseSetup<TSubject, TDerivedClass> where TDerivedClass : BaseSetup<TSubject, TDerivedClass>
    {
        private readonly IDataService<TSubject> _dataService;

        private readonly List<TSubject> _itemsToCreate = new();
        public TSubject LastAdded { get { return _itemsToCreate.Last(); } }

        public List<TSubject> Created = new();
        protected BaseSetup(IDataService<TSubject> dataService)
        {
            _dataService = dataService;
        }

        public TDerivedClass Add()
        {
            return Add(Get());
        }

        public TDerivedClass Add(int amount)
        {
            for (int i = 0; i < amount; i++)
                Add(Get());

            return (TDerivedClass)this;
        }

        public TDerivedClass Add(TSubject subject)
        {
            _itemsToCreate.Add(subject);

            return (TDerivedClass)this;
        }

        public abstract TSubject Get();

        public List<TSubject> Get(int amount)
        {
            var result = new List<TSubject>();
			for (int i = 0; i < amount; i++)
			{
				var item = Get();
				result.Add(item);
				Add(item);
			}

        	return result;
        }

        public TDerivedClass Persist()
        {
            foreach (var subject in _itemsToCreate)
            {
                _dataService.Create(subject);
                Created.Add(subject);
            }

            _itemsToCreate.Clear();

            return (TDerivedClass)this;
        }
    }
}
