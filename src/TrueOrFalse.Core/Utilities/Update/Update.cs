using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Updates
{
    public class Update
    {
        public void Run()
        {
            int _currentVersion = GetCurrentVersion();

            if (_currentVersion < 1) UpdateToVs1.Run();
            if (_currentVersion < 2) UpdateToVs2.Run();
            if (_currentVersion < 3) UpdateToVs3.Run();
        }

        private int GetCurrentVersion()
        {
            return 0;
        }
    }
}
