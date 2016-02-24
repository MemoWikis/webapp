using System;
using Newtonsoft.Json;

[Serializable]
public class MainLicenseInfo
{
    public int MainLicenseId;
    public string Author;
    public string Markup;
    public DateTime MarkupDownloadDate;

    public License GetMainLicense()
    {
        return LicenseRepo.GetById(MainLicenseId);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static MainLicenseInfo FromJson(string json)
    {
        return JsonConvert.DeserializeObject<MainLicenseInfo>(json ?? "") ?? new MainLicenseInfo();
    }
}
