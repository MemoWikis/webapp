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
        private readonly DbSettingsLoader _dbSettingsLoader;
        private readonly Dictionary<int, Action> _actions = new Dictionary<int, Action>();

        public UpdateStepExcecuter(DoesTableExist doesTableExist, DbSettingsLoader dbSettingsLoader)
        {
            _doesTableExist = doesTableExist;
            _dbSettingsLoader = dbSettingsLoader;
        }

        public UpdateStepExcecuter Add(int stepNo, Action action)
        {
            _actions.Add(stepNo, action);
            return this;
        }

        public void Run()
        {
            var dbSettings = _dbSettingsLoader.Get();

            int currentVersion = _doesTableExist.Run("Setting") ? 0 : dbSettings.AppVersion.Value;

            foreach (var dictionaryItem in _actions)
                if (currentVersion > dictionaryItem.Key)
                    return;
                else
                {
                    dictionaryItem.Value();
                    _dbSettingsLoader.Update(dbSettings);
                }
                    
        }
    }
}
