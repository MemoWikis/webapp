using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class MainLicenseInfo
{
    public int MainLicenseId;
    public string Author;
    public string Markup;
    public DateTime MarkupDownloadDateTime;

    [JsonIgnore]
    public License MainLicense;

    public MainLicenseInfo()
    {
        LicenseRepository.GetById(MainLicenseId);
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
