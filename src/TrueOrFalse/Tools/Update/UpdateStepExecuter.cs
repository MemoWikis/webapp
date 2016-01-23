using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Updates
{
    public class UpdateStepExecuter : IRegisterAsInstancePerLifetime
    {
        private readonly DbSettingsRepository _dbSettingsRepository;
        private readonly Dictionary<int, Action> _actions = new Dictionary<int, Action>();

        public UpdateStepExecuter(DbSettingsRepository dbSettingsRepository){
            _dbSettingsRepository = dbSettingsRepository;
        }

        public UpdateStepExecuter Add(Action action)
        {
            var declaringType = action.GetMethodInfo().DeclaringType;
            if (declaringType == null)
                throw new Exception("no declaring type - stepNo overload");       

            var typeName = declaringType.Name;

            var captures = Regex.Match(typeName, "[0-9]*$").Captures;
            if(captures.Count != 1)
                throw new Exception("type does not end with a number '" + typeName + "'");

            _actions.Add(Convert.ToInt32(captures[0].Value), action);
            return this;
        }

        public UpdateStepExecuter Add(int stepNo, Action action)
        {
            _actions.Add(stepNo, action);
            return this;
        }

        public void Run()
        {                
            var dbSettings = _dbSettingsRepository.Get();

            foreach (var dictionaryItem in _actions)
                if (dbSettings.AppVersion < dictionaryItem.Key)
                {
                    Logg.r().Information("update to {0} - START", dictionaryItem.Key);
                    dictionaryItem.Value();
                    Logg.r().Information("update to {0} - END", dictionaryItem.Key);
                    _dbSettingsRepository.UpdateAppVersion(dictionaryItem.Key);
                }   
        }
    }
}