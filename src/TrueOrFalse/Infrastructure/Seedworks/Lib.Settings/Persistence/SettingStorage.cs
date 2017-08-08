using System;
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

        public void CreateOrUpdate(IList<Setting> settings)
        {
            foreach (var setting in settings)
                CreateOrUpdate(setting);
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

        public void CreateOrUpdate(Setting setting)
        {
            if (setting.DateCreated == DateTime.MinValue)
                setting.DateCreated = DateTime.Now;

            setting.DateModified = DateTime.Now;

            _repository.CreateOrUpdate(setting);
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

        public void Delete(IList<Setting> settings)
        {
            foreach (var setting in settings)
                Delete(setting);
        }

        public IList<Setting> GetAll()
        {
            return _repository.GetAll();
        }

        private IList<Setting> _allSettings;
        public IList<Setting> GetAllCached()
        {
            if (_allSettings == null) _allSettings = GetAll();
            return _allSettings;
        }

        public IList<Setting> GetBy(SettingSearchDesc searchDesc)
        {
            return _repository.GetBy(searchDesc);
        }

		/// <summary>
		/// Convenience method to get a single setting.
		/// </summary>
		/// <param name="settingSearchDesc"></param>
		/// <returns></returns>
		public T GetUnique<T>(SettingSearchDesc settingSearchDesc) where T : Setting, new()
		{
			return _repository.GetUnique(settingSearchDesc) as T;
		}
    }
}
