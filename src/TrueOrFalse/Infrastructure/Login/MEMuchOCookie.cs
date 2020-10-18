using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

public class MemuchoCookie
{
    public static HttpCookie GetNew()
    {
        var cookie = new HttpCookie(Settings.MemuchoCookie);
        cookie.Expires = DateTime.Now.AddDays(45);

        return cookie;
    }


    public static HttpCookie SettingsGetNew(string key, string value)
    {
        var cookie = new HttpCookie(Settings.MemuchoCookie);
        cookie.Expires = DateTime.Now.AddDays(45);

        return cookie;
    }
}

public class SettingObject
{
    public string Key { get; set; }
    public string Value { get; set; }
}