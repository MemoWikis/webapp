using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Language
{
    public static string SingularPlural(int numberItems, string singular, string plural)
    {
        return numberItems == 1 ? singular : plural;
    }
}
