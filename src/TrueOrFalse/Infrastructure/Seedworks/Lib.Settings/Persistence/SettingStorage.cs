using System.Collections.Generic;
using Seedworks.Lib.Persistence;

namespace Seedworks.Lib.Settings
{
    public class SettingStorage : IDataService<Setting>
    {
        private readonly ISettingRepository _repository;

        public SettingStorage(ISettingRepository repository)
        {
            _repository = repository;
        }

        public void Create(IList<Setting> settings)
        {
            foreach (var setting in settings)
                Create(setting);
        }

        public void Create(Setting list)
        {
            list.DateCreated = list.DateModified = DateTime.Now;

            _repository.Create(list);
        }

        public void Update(Setting setting)
        {
            setting.DateModified = DateTime.Now;
            _repository.Update(setting);
        }

        public void Update(IList<Setting> settings)
        {
            foreach(var setting in settings)
                Update(setting);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        IList<Setting> IDataService<Setting>.GetAll()
        {
            return GetAll();
        }

        public Setting GetById(int id)
        {
            throw new System.NotImplementedException();
        }

    	IList<Setting> IDataService<Setting>.GetBy(ISearchDesc searchDesc)
        {
    		return _repository.GetBy((SettingSearchDesc) searchDesc);
        }

        public void Delete(Setting setting)
        {
            _repository.Delete(setting);
        }

        public IList<Setting> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Setting> GetBy(SettingSearchDesc searchDesc)
        {
            return _repository.GetBy(searchDesc);
        }
    }
}
