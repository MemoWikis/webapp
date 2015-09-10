public class Language
{
    public static string SingularPlural(int numberItems, string singular, string plural)
    {
        return numberItems == 1 ? singular : plural;
    }
}
