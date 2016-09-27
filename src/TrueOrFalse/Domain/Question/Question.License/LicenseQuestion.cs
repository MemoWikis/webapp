using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LicenseQuestion
{
    public int Id;

    public string NameShort;

    public string NameLong;

    public string LicenseText;

    public bool IsDefault()
    {
        return Id == LicenseQuestionRepo.DefaultLicenseId;
    }
}
