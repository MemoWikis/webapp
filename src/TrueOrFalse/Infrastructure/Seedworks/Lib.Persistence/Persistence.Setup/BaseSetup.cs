using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace Seedworks.Lib.Persistence
{
    public abstract class BaseSetup<TSubject, TDerivedClass> where TDerivedClass : BaseSetup<TSubject, TDerivedClass>
    {
        private readonly IDataService<TSubject> _dataService;

        private readonly List<TSubject> _itemsToCreate = new List<TSubject>();
        public TSubject LastAdded { get { return _itemsToCreate.Last(); } }

        public List<TSubject> Added { get { return _itemsToCreate; } }
        public List<TSubject> Created = new List<TSubject>();
        public TSubject LastCreated { get { return Created.Last(); } }

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

        public TSubject GetPersisted()
        {
            Add().Persist();
            return Created.Last();
        }

        public TSubject GetPersisted(Func<TSubject, TSubject> modifier)
        {
            Add();
            var subject = modifier(LastAdded);
            Persist();
            return subject;
        }

        public TSubject GetPersisted(Action<TSubject> modifier)
        {
            Add();
            var subject = LastAdded;
        	modifier(subject);
            Persist();
            return subject;
        }

        public List<TSubject> GetPersisted(int amount)
        {
            var subjects = Get(amount);
            Persist();
            return subjects;
        }
    }
}
