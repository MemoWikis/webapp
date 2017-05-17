using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SponsorRepo{

    public static List<Sponsor> GetAllSponsors() {
        var allSponsors = new List<Sponsor>
        {
            new Sponsor {
                Id = 1,
                ImpressionSharePercentage = 0.5,
                SponsorUrl = "http://www.tutory.de",
                ImageUrl = "/Images/Sponsors/tutory.png",
                LinkText = "tutory",
                TextBeforeLink = "Zum Erstellen von Arbeitsblättern empfehlen wir "
            },

            new Sponsor
            {
                Id = 2,
                ImpressionSharePercentage = 0.1,
                SponsorUrl = "http://greensoeasy.com/",
                ImageUrl = "/Images/Sponsors/greensoeasy.png",
                LinkText = "greensoeasy",
                TextBeforeLink = "Selbst etwas für das Klima tun mit der App von "
            },

            new Sponsor {
                Id = 3,
                ImpressionSharePercentage = 0.4,
                SponsorUrl = "https://learning-levelup.de/",
                ImageUrl = "/Images/LogosPartners/Logo_LearningLevelUp.png",
                LinkText = "Learning Level Up",
                TextBeforeLink = "Für anpassbare digitale Lehr- und Lerninhalte als Animation, Grafik und Video empfehlen wir ",
                ImageStyleOverwrite = "max-width: 70%"
            },

        };

        if (allSponsors.Sum(s => s.ImpressionSharePercentage) > 1)
        {
            throw new Exception("Overall probability exceeds 1.0");
        }

        return allSponsors;
    }

    public static Sponsor GetSponsor()
    {
        //Code snippet from http://www.vcskicks.com/random-element.php:
        Random r = new Random();
        double diceRoll = r.NextDouble();
        var allSponsors = GetAllSponsors();
        var pickedSponsor = GetSponsorById(1);

        double cumulative = 0.0;
        for (int i = 0; i < allSponsors.Count; i++)
        {
            cumulative += allSponsors[i].ImpressionSharePercentage;
            if (diceRoll < cumulative)
            {
                pickedSponsor = allSponsors[i];
                break;
            }
        }

        return pickedSponsor;
    }

    public static Sponsor GetSponsorById(int id)
    {
        return GetAllSponsors().FirstOrDefault(s => s.Id == id);
    }
}
