using System.Collections.Generic;

public class BadgeTypes
{
    private static IList<BadgeType> _allBadgeTypes;
    public static IList<BadgeType> All()
    {
        return _allBadgeTypes ?? (_allBadgeTypes = new List <BadgeType>
        {
            //FirstSteps
            new BadgeType
            {
                Key = "FirstHourSupporter",
                Name = "Fördermitglied der 1. Stunde",
                Description = "Während des 1. Jahres Fördermitglied geworden (und für ein Jahr geblieben)",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()},
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = filterParams => new BadgeAwardCheckResult {Success = false},
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Nutzer der 1. Stunde",
                Description = "Während der Beta-Phase Nutzer geworden",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.Registration },
                AwardCheck = BadgeAwardCheck.AlwaysFalse(),
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Beta-Berater",
                Description = "Per Hand verliehen an alle, die während der Beta-Phase MEMuchO genutzt und uns beraten haben",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetSilver()},
                BadgeCheckOn = new []{ BadgeCheckOn.Manually },
                AwardCheck = filterParams => new BadgeAwardCheckResult {Success = false},
            },
            new BadgeType
            {
                Key = "NewbieBronze",
                Name = "Newbie",
                Description = "1 Frage im Wuwi, 1 Frage beantwortet",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.Answer, BadgeCheckOn.WishKnowledgeAdd},
                AwardCheck = BadgeAwardCheck.Get(filterParams =>
                {
                    if (filterParams.WishknowledgeCount() >= 1 && filterParams.AnswerCount() >= 1)
                        return BadgeLevel.GetBronze();

                    return null;
                }),
            },
            new BadgeType
            {
                Key = "NewbieSilver",
                Name = "Newbie",
                Description = "2 Multiple-Choice-Fragen mit Kategorie erstellt, 2 Fragesätze mit mind. 10 Fragen erstellt, 2 fremde und 2 eigene Fragen in Wunschwissen aufgenommen",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetSilver()},
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate, BadgeCheckOn.SetUpdateOrCreate, BadgeCheckOn.WishKnowledgeAdd},
            },
            new BadgeType
            {
                Key = "NewbieGold",
                Name = "Newbie",
                Description = "wie Silber, dazu: 3 Spiele gespielt; 3 Termine angelegt; 3 Nutzern gefolgt, 30 Fragen im Wuwi, 1 Kommentar geschrieben, 1 Kategorie erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()},
                BadgeCheckOn = new []{ BadgeCheckOn.GameFinished, BadgeCheckOn.DateCreated, BadgeCheckOn.UserFollowed, BadgeCheckOn.WishKnowledgeAdd, BadgeCheckOn.CommentedAdded, BadgeCheckOn.CategoryUpdateOrCreate },
            },
            new BadgeType
            {
                Key = "HelloWorld",
                Name = "Hello World",
                Description = "Vollständiges Profil",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.UserProfileUpdated },
            },
            
            //Questions
            new BadgeType
            {
                Key = "1000Words",
                Name = "MehrAls1000Worte",
                Description = "{badgePoints} Fragen mit Bildern erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Questions),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
            },

            new BadgeType
            {
                Key = "ABCD",
                Name = "ABCD",
                Description = "{badgePoints} Multiple-Choice-Fragen erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Questions),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
            },

            new BadgeType
            {
                Key = "KnowItAll",
                Name = "Allgemeinwisser",
                Description = "eine eigene Frage wurde {badgePoints} Mal in fremdes Wuwi aufgenommen",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Questions),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd },
            },
            
            //Sets
            new BadgeType
            {
                Key = "DonDoItSelf",
                Name = "IchMachsDochNichtSelbst",
                Description = "{badgePoints} Fragesätze mit mehr als 10 Fragen, mit mind. 1 fremden Frage zusammengestellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Sets),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.SetUpdateOrCreate }
            },

            //Categories
            new BadgeType
            {
                Key = "FamilyFriend",
                Name = "Familienfreund",
                Description = "{badgePoints} Eltern-/Kindkategorien verknüpft",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(20),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(400)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.CategoryUpdateOrCreate }
            },

            new BadgeType
            {
                Key = "Expert",
                Name = "VomFach",
                Description = "{badgePoints} Fragen zu einer Kategorie erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(10),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate }
            },

            new BadgeType
            {
                Key = "Universalist",
                Name = "Universalist",
                Description = "{badgePoints} verschiedene Kategorien zu eigenen Fragen zugeordnet",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate }
            },

            //WishKnowledge
            new BadgeType
            {
                Key = "WannaKnow",
                Name = "WillsWissen",
                Description = "{badgePoints} Fragen im eigenen Wunschwissen hinzugefügt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(200),
                    BadgeLevel.GetGold(1000)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd }
            },

            new BadgeType
            {
                Key = "Archaeologist",
                Name = "Archäologe",
                Description = "{badgePoints} Fragen zum Wuwi hinzugefügt, die älter als 2 Jahre sind",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd }
            },

            new BadgeType
            {
                Key = "FasterThanShadow",
                Name = "SchnellerAlsMeinSchatten",
                Description = "{badgePoints} fremde Fragen zum Wunschwissen hinzugefügt, die neuer als 24h sind.",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(20),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd }
            },

            new BadgeType
            {
                Key = "ThanksForHelp",
                Name = "DankeFürDieHilfe",
                Description = "{badgePoints} fremde Fragen im eigenen Wuwi",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd }
            },

            //Training
            new BadgeType
            {
                Key = "Swot",
                Name = "Streber",
                Description = "{badgePoints} Tage am Stück gelernt [nicht: Frage beantwortet]",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(3),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.AnswerInLearningSession }
            },

            new BadgeType
            {
                Key = "OldStager",
                Name = "Alter Hase",
                Description = "an insgesamt {badgePoints} Tagen gelernt [nicht: Frage beantwortet]",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(3),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(300)
                }
            },
        });
    }
}