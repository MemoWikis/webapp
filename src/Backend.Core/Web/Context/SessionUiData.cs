public class SessionUiData : IRegisterAsInstancePerLifetime
{
    private readonly SessionData _sessionData;

    public SessionUiData(SessionData sessionData)
    {
        _sessionData = sessionData;
    }

    public TmpImageStore TmpImagesStore => _sessionData.Get("tmpImageStore", new TmpImageStore());
}