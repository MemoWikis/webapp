using System;
using System.Collections.Generic;

[Serializable]
public class SignalRAuthInfo
{
    public string CookieToken;
    public int UserId;
    public List<string> ConnectionIds;
}