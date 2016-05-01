using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

/// <remarks>
/// http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c
/// </remarks>
public class MutexX : IDisposable
{
    public bool hasHandle = false;
    Mutex mutex;

    private void InitMutex(string nameSuffix)
    {
        string appGuid = ((GuidAttribute)Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
        string mutexId = $"Global\\{{{appGuid}-{nameSuffix}}}";
        mutex = new Mutex(false, mutexId);

        var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        var securitySettings = new MutexSecurity();
        securitySettings.AddAccessRule(allowEveryoneRule);
        mutex.SetAccessControl(securitySettings);
    }

    public MutexX(int timeOut, string nameSuffix)
    {
        InitMutex(nameSuffix);
        try
        {
            if (timeOut < 0)
                hasHandle = mutex.WaitOne(Timeout.Infinite, false);
            else
                hasHandle = mutex.WaitOne(timeOut, false);

            if (hasHandle == false)
                throw new TimeoutException("Timeout waiting for exclusive access on MutexX");
        }
        catch (AbandonedMutexException)
        {
            hasHandle = true;
        }
    }

    public void Dispose()
    {
        if (mutex != null)
        {
            if (hasHandle)
                mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }
}