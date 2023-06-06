using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Updates
{
    public class UpdateStepExecuter : IRegisterAsInstancePerLifetime
    {
        private readonly DbSettingsRepo _dbSettingsRepo;
        private readonly Dictionary<int, Action> _actions = new Dictionary<int, Action>();

        public UpdateStepExecuter(DbSettingsRepo dbSettingsRepo){
            _dbSettingsRepo = dbSettingsRepo;
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
            var appVersion = _dbSettingsRepo.GetAppVersion();

            foreach (var dictionaryItem in _actions)
                if (appVersion < dictionaryItem.Key)
                {
                    Logg.r().Information("update to {0} - START", dictionaryItem.Key);
                    dictionaryItem.Value();
                    Logg.r().Information("update to {0} - END", dictionaryItem.Key);
                    _dbSettingsRepo.UpdateAppVersion(dictionaryItem.Key);
                }   
        }
    }
}