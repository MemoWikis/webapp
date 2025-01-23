using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

public class MutexX : IDisposable
{
    public bool hasHandle = false;
    private Mutex _mutex;

    public MutexX(int timeOut, string nameSuffix)
    {
        if (Debugger.IsAttached)
            timeOut = -1;

        InitMutex(nameSuffix);

        try
        {
            // WaitOne returns true if the mutex was obtained, false if timed out
            if (timeOut < 0)
                hasHandle = _mutex.WaitOne(Timeout.Infinite, false);
            else
                hasHandle = _mutex.WaitOne(timeOut, false);

            if (!hasHandle)
                throw new TimeoutException("Timeout waiting for exclusive access on MutexX");
        }
        catch (AbandonedMutexException)
        {
            // The mutex was abandoned by another process/thread.
            // We still consider it acquired.
            hasHandle = true;
        }
    }

    private void InitMutex(string nameSuffix)
    {
        // Attempt to retrieve the GuidAttribute from the executing assembly
        var guidAttributes = Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(GuidAttribute), false);

        // If none found, generate a fallback GUID to avoid IndexOutOfRangeException
        string appGuid;
        if (guidAttributes.Length == 0)
        {
            // Use a random GUID fallback
            appGuid = Guid.NewGuid().ToString();
        }
        else
        {
            // Use the assembly-level GUID
            appGuid = ((GuidAttribute)guidAttributes[0]).Value;
        }

        // Construct a unique mutex ID
        string mutexId = $"{{{appGuid}-{nameSuffix}}}";

        // Create the named mutex (initially not owned)
        _mutex = new Mutex(initiallyOwned: false, name: mutexId);
    }

    public void Dispose()
    {
        if (_mutex != null)
        {
            if (hasHandle)
            {
                _mutex.ReleaseMutex();
            }
            _mutex.Dispose();
        }
    }
}
