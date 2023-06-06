using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LicenseQuestion
{
    public int Id;

    public string NameLong;

    public string NameShort;

    public string DisplayTextShort;
    public string DisplayTextFull;

    public string LicenseLink;

    public string LicenseShortDescriptionLink;

    public bool? AuthorRequired;
    public bool? LicenseLinkRequired;
    public bool? ChangesNotAllowed;

    public bool IsDefault()
    {
        return Id == LicenseQuestionRepo.DefaultLicenseId;
    }
}
