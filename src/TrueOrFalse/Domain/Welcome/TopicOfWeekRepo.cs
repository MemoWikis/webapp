using System.Collections.Generic;
using System;
using System.Linq;

public class TopicOfWeekRepo
{

    public static List<TopicOfWeek> GetAllTopicsOfWeek()
    {
        var topicsOfWeek = new List<TopicOfWeek>
        {
            new TopicOfWeek
            {
                FirstValidDay = new DateTime(2017,07,17),
                CategoryId = 392, // Sportbootführerscheine
                TopicOfWeekTitle = "Sportbootführerscheine",
                TopicDescriptionHTML = @"
                    <p>
                        Auf Gewässern in Deutschland können unmotorisierte und kleine Boote in der Regel einfach so gefahren werden.
                        Für bestimmte Boote benötigt man allerdings bestimmte &quot;Führerscheine&quot;. Unter Hobby-Seglern sind vor allem
                        die Sportbootführerscheine Binnen bzw. See beliebt, kurz auch SBF Binnen und SBF See genannt. Sie werden benötigt,
                        um Sportboote mit mehr als 11,03 kW (15 PS) auf Binnenschifffahrtsstraßen (dafür gibt es den SBF Binnen)
                        oder auf Seeschiffahrtsstraßen zu führen.
                    </p>
                    <p>
                        Beim SBF Binnen ist zusätzlich die Länge der Boote auf 15 Meter beschränkt. Diese Beschränkung gibt es beim SBF See nicht. 
                        Der ist notwendig, wenn du in der der Drei-Seemeilen-Zone und im Fahrwasser innerhalb der Zwölf-Seemeilen-Zone Boot fahren möchtest.
                        Beide Sportbootführerscheine sind unabhängig voneinander, bauen also nicht aufeinander auf und setzen sich auch nicht voraus. 
                        Allerdings sind die 72 Basisfragen in beiden Theorieprüfungen gleich. 
                        In beiden Prüfungen geht es zuerst um die Theorie und dann um die Praxis. Für die Theorieprüfung gibt es jeweils einen Pool aus 
                        300 offiziellen Fragen, die du mit memucho lernen kannst. Jetzt musst du nur noch entscheiden, ob du lieber durch die Seen und Flüsse
                        schipperst (<a href='/Sportbootfuehrerschein-Binnen/393'>zur Themenseite SBF Binnen</a>) oder ob es gleich auf die Ost- oder Nordsee 
                        gehen soll (<a href='/Sportbootfuehrerschein-See/395'>zur Themenseite SBF See</a>).
                    </p>",
                QuizOfWeekSetId = 50, //SBF Binnen: Segeln
                AdditionalCategoriesIds = new List<int> { 393, 395, 614, 633 }
            },

            new TopicOfWeek
            {
                FirstValidDay = new DateTime(2017,07,24),
                CategoryId = 264, // Psychologie (Studium)
                TopicOfWeekTitle = "Psychologie",
                TopicDescriptionHTML = @"
                    <p>
                        Die Psychologie ist eine Wissenschaft, die sich mit den seelischen Vorgängen im Menschen befasst.
                        Sie ist nicht nur ein sehr beliebtes Studienfach, sondern auch ein sehr beliebtes populärwissenschaftliches Thema.
                        Etwas abfällig oder (selbst-)ironisch spricht man von &quot;Küchenpsychologie&quot;, wenn es um eine
                        naive und unreflektierte Verwendung alltagspsychologischer Kenntnisse geht. Für andere ist die Psychologie
                        vor allem die Kunst, Menschen einzuschätzen und zu lenken.
                    </p>
                    <p>
                        Dabei hat die Psychologie tatsächlich eine Vielzahl an spannenden Forschungsfeldern und Erkenntnissen zu bieten.
                        Wie vielfältig das Fach ist, zeigt unsere <a href='/Psychologie-Studium/264'>Themenseite zum Studienfach Psychologie</a>: 
                        Von der Lern -, Bio - oder Entwicklungspsychologie über die Arbeits-und Organisations - Psychologie bis zur Klinischen Psychologie. 
                        Ein weiterer Schwerpunkt in dem Fach sind die <a href= '/Statistik-fuer-PsychologInnen/649'>Methoden</a>,
                        denn viele Erkenntnisse werden über quantitative Studien gewonnen.
                    </p>",
                QuizOfWeekSetId = 141,
                AdditionalCategoriesIds = new List<int> { 271, 607, 618, 649 }
            },

            new TopicOfWeek
            {
                FirstValidDay = new DateTime(2017,07,31),
                CategoryId = 182, // Hauptstadt
                TopicDescriptionHTML = @"",
                QuizOfWeekSetId = 20,
                AdditionalCategoriesIds = new List<int> { 752, 437, 439, 146}
            },

            new TopicOfWeek
            {
                FirstValidDay = new DateTime(2017,08,07),
                CategoryId = 145, // Europäische Union
                TopicDescriptionHTML = @"",
                QuizOfWeekSetId = 19,
                AdditionalCategoriesIds = new List<int> { 847, 844, 387, 358 }
            },

            new TopicOfWeek
            {
                FirstValidDay = new DateTime(2017,08,14),
                CategoryId = 794, // Geschichte
                TopicDescriptionHTML = @"",
                QuizOfWeekSetId = 90,
                AdditionalCategoriesIds = new List<int> { 828, 827, 829, 830 }
            },

            new TopicOfWeek
            {
                FirstValidDay = new DateTime(2017,08,21),
                CategoryId = 363, // Globalisierung
                TopicDescriptionHTML = @"",
                QuizOfWeekSetId = 59,
                AdditionalCategoriesIds = new List<int> { 364, 847, 14, 748 }
            },

        };
        return topicsOfWeek;
    }

    public static TopicOfWeek GetTopicOfWeek(DateTime date)
    {
        return GetAllTopicsOfWeek().OrderByDescending(t => t.FirstValidDay).FirstOrDefault(t => date.Date >= t.FirstValidDay.Date);
    }


}
