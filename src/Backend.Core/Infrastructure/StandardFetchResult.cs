public class StandardFetchResult<T>
{
    public bool success = true;
    public T data;
    public string key;

    public StandardFetchResult(T value, string exceptionMessage = "")
    {
        if (value == null)
        {
            success = false;
            key = exceptionMessage;
        }
    }
}