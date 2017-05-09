using System.Collections.Generic;
using System.Linq;

public class LicenseQuestionRepo
{
    public const int DefaultLicenseId = 1;

    public static List<LicenseQuestion> GetAllRegisteredLicenses()
    {
        return new List<LicenseQuestion>()
        {
            //Don't change IDs!
           
            new LicenseQuestion
            {
                Id = 1,
                NameLong = "Creative Commons: Namensnennung 4.0 International",
                NameShort = "CC BY 4.0",
                LicenseLink = @"https://creativecommons.org/licenses/by/4.0/legalcode",
                LicenseShortDescriptionLink = @"https://creativecommons.org/licenses/by/4.0/deed.de",
                DisplayTextShort = "CC BY 4.0", //should/could be the image
                DisplayTextFull = @"Dieser Beitrag steht unter der Lizenz 'Creative Commons: Namensnennung 4.0 International'. Du kannst ihn frei verwenden und ver�ndern, solange du den Autor angibst und die Lizenz verlinkst 
                                    (vgl. <a href='https://creativecommons.org/licenses/by/4.0/deed.de' target='_blank'>die Kurzfassung</a>
                                    und <a href='https://creativecommons.org/licenses/by/4.0/legalcode' target='_blank'>den ausf�hrlichen Lizenztext</a>).",
                AuthorRequired = true,
                LicenseLinkRequired = true,
                ChangesNotAllowed = false
            },

            new LicenseQuestion
            {
                Id = 2,
                NameLong = "Anderes Amtliches Werk, Quelle BAMF",
                NameShort = "Amtliches Werk BAMF",
                DisplayTextShort = "Quelle: BAMF",
                DisplayTextFull = "Dieser Beitrag stellt ein 'anderes amtliches Werk' gem�� <a href='https://www.gesetze-im-internet.de/urhg/__5.html' target='_blank'>� 5 Abs. 2 des Urheberrechtsgesetzes</a> dar. Es darf frei verwendet werden, allerdings unter Angabe der Quelle und ohne Ver�nderungen.",
                LicenseLink = "https://www.gesetze-im-internet.de/urhg/__5.html",
                AuthorRequired = true,
                LicenseLinkRequired = false,
                ChangesNotAllowed = true
            },

            new LicenseQuestion
            {
                Id = 3,
                NameLong = "Elektronischer Wasserstra�en-Informationsservice (ELWIS.de)",
                NameShort = "ELWIS",
                DisplayTextShort = "Quelle: ELWIS.de",
                DisplayTextFull = "Die <a href='https://www.elwis.de/Freizeitschifffahrt/fuehrerscheininformationen/' target='_blank'>Frage und die Antwortm�glichkeiten</a> " +
                                  "entstammen dem Elektronischen Wasserstra�en-Informationsservice (ELWIS.de) der Wasserstra�en- und Schifffahrtsverwaltung des Bundes.",
                LicenseLink = "https://www.elwis.de/misc/disclaimer.html",
                AuthorRequired = true,
                LicenseLinkRequired = false,
                ChangesNotAllowed = true
            },

            new LicenseQuestion
            {
                Id = 4,
                NameLong = "Bund/L�nder-Arbeitsgemeinschaft Chemikaliensicherheit (BLAC)",
                NameShort = "BLAC",
                DisplayTextShort = "Quelle: BLAC",
                DisplayTextFull = "Die Frage, Antwortm�glichkeiten und die Referenz auf Fundstellen entstammen dem " +
                                  "Gemeinsamen Fragenkatalog der L�nder (GFK) f�r die Sachkundepr�fung nach � 5 der Chemikalien-Verbotsverordnung " +
                                  "und sind gesammelt in <a href='http://blak-uis.server.de/servlet/is/2146/P-4a.pdf/' target='_blank'>diesem PDF</a> verf�gbar. " +
                                  "Die Nutzung des Fragenkatalogs ist nach Ma�gabe von � 62 Abs. 1 bis 3 (�nderungsverbot) und � 63 Abs. 1 und 2 (Quellenangabe) UrhG zul�ssig. " +
                                  "Autor ist die <a href='http://www.blac.de/servlet/is/2146/' target='_blank'>BLAC</a>.",
                LicenseLink = "http://blak-uis.server.de/servlet/is/2146/P-4a.pdf",
                AuthorRequired = true,
                LicenseLinkRequired = false,
                ChangesNotAllowed = true
            }
        };

    }

    public static LicenseQuestion GetById(int id)
    {
        return GetAllRegisteredLicenses().FirstOrDefault(license => license.Id == id);
    }

    public static LicenseQuestion GetDefaultLicense()
    {
        return GetAllRegisteredLicenses().Single(l => l.IsDefault());
    }
}

