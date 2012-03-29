using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateStepExcecuter : IRegisterAsInstancePerLifetime
    {
        private readonly DoesTableExist _doesTableExist;
        private readonly DbSettingsRepository _dbSettingsRepository;
        private readonly Dictionary<int, Action> _actions = new Dictionary<int, Action>();

        public UpdateStepExcecuter(DoesTableExist doesTableExist, DbSettingsRepository dbSettingsRepository)
        {
            _doesTableExist = doesTableExist;
            _dbSettingsRepository = dbSettingsRepository;
        }

        public UpdateStepExcecuter Add(int stepNo, Action action)
        {
            _actions.Add(stepNo, action);
            return this;
        }

        public void Run()
        {
            var dbSettings = _dbSettingsRepository.Get();

            int currentVersion = _doesTableExist.Run("Setting") ? 0 : dbSettings.AppVersion;

            foreach (var dictionaryItem in _actions)
                if (currentVersion > dictionaryItem.Key)
                    return;
                else
                {
                    dictionaryItem.Value();
                    _dbSettingsRepository.Update(dbSettings);
                }
                    
        }
    }
}
