using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

/// http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c
/// </remarks>
public class MutexX : IDisposable
{
    public bool hasHandle = false;
    Mutex _mutex;
    private void InitMutex(string nameSuffix)
    {
        string appGuid = ((GuidAttribute)Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;

        string mutexId = $"{{{appGuid}-{nameSuffix}}}";

        _mutex = new Mutex(false, mutexId);
    }

    public MutexX(int timeOut, string nameSuffix)
    {
        if (Debugger.IsAttached)
            timeOut = -1;

        InitMutex(nameSuffix);
        try
        {
            if (timeOut < 0)
                hasHandle = _mutex.WaitOne(Timeout.Infinite, false);
            else
                hasHandle = _mutex.WaitOne(timeOut, false);

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
        if (_mutex != null)
        {
            if (hasHandle)
                _mutex.ReleaseMutex();
            _mutex.Dispose();
        }
    }
}