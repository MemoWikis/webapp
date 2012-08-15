using System;
using System.Collections.Generic;
using Gibraltar.Agent;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateStepExecuter : IRegisterAsInstancePerLifetime
    {
        private readonly DoesTableExist _doesTableExist;
        private readonly DbSettingsRepository _dbSettingsRepository;
        private readonly Dictionary<int, Action> _actions = new Dictionary<int, Action>();

        public UpdateStepExecuter(DoesTableExist doesTableExist, DbSettingsRepository dbSettingsRepository)
        {
            _doesTableExist = doesTableExist;
            _dbSettingsRepository = dbSettingsRepository;
        }

        public UpdateStepExecuter Add(int stepNo, Action action)
        {
            _actions.Add(stepNo, action);
            return this;
        }

        public void Run()
        {
            if(!_doesTableExist.Run("Setting")){
                UpdateToVs001InitialStep.Run();
            }
                
            var dbSettings = _dbSettingsRepository.Get();

            foreach (var dictionaryItem in _actions)
                if (dbSettings.AppVersion < dictionaryItem.Key)
                {
                    Log.TraceInformation(String.Format("update to {0} - START", dictionaryItem.Key));
                    dictionaryItem.Value();
                    Log.TraceInformation(String.Format("update to {0} - END", dictionaryItem.Key));
                    _dbSettingsRepository.UpdateAppVersion(dictionaryItem.Key);
                }   
        }
    }
}