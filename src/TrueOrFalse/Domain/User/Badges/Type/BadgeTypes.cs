using System.Collections.Generic;
using System.Linq;
using NHibernate;

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
                Name = "F�rdermitglied der 1. Stunde",
                Description = "W�hrend des 1. Jahres F�rdermitglied geworden (und f�r ein Jahr geblieben)",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()},
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = filterParams => new BadgeAwardCheckResult {Success = false},
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Nutzer der 1. Stunde",
                Description = "W�hrend der Beta-Phase Nutzer geworden",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.Registration },
                AwardCheck = BadgeAwardCheck.AlwaysFalse(),
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Beta-Berater",
                Description = "Per Hand verliehen an alle, die w�hrend der Beta-Phase MEMuchO genutzt und uns beraten haben",
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
                AwardCheck = BadgeAwardCheck.Get(fp => 
                    fp.WishknowledgeCount() >= 1 && fp.AnswerCount() >= 1
                        ? BadgeLevel.GetBronze()
                        : null),
            },
            new BadgeType
            {
                Key = "NewbieSilver",
                Name = "Newbie",
                Description = "2 Multiple-Choice-Fragen mit Kategorie erstellt, 2 Frages�tze mit mind. 10 Fragen erstellt, 2 fremde und 2 eigene Fragen in Wunschwissen aufgenommen",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetSilver()},
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate, BadgeCheckOn.SetUpdateOrCreate, BadgeCheckOn.WishKnowledgeAdd},
                AwardCheck = BadgeAwardCheck.Get(fp =>
                    fp.Questions_MultipleChoice_WithCategories() >= 2 &&
                    fp.SetsWithAtLeast10Questions().Count() >= 2 &&
                    fp.Wishknowledge_UserIsCreator() >= 2 &&
                    fp.Wishknowledge_OtherIsCreator() >= 2
                        ? BadgeLevel.GetSilver()
                        : null),
            },
            new BadgeType
            {
                Key = "NewbieGold",
                Name = "Newbie",
                Description = "wie Silber, dazu: 3 Spiele gespielt; 3 Termine angelegt; 3 Nutzern gefolgt, 30 Fragen im Wuwi, 1 Kommentar geschrieben, 1 Kategorie erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()},
                BadgeCheckOn = new []{ BadgeCheckOn.GameFinished, BadgeCheckOn.DateCreated, BadgeCheckOn.UserFollowed, BadgeCheckOn.WishKnowledgeAdd, BadgeCheckOn.CommentedAdded, BadgeCheckOn.CategoryUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.Get(fp => 
                    fp.IsBadgeAwarded("NewbieSilver", fp) &&
                    fp.PlayedGames() >= 3 &&
                    fp.Dates() >= 3 &&
                    fp.UsersFollowing() >= 3
                        ? BadgeLevel.GetGold() 
                        : null)
            },
            new BadgeType
            {
                Key = "HelloWorld",
                Name = "Hello World",
                Description = "Vollst�ndiges Profil",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.UserProfileUpdated },
                /* NOT-DONE AwardCheck = BadgeAwardCheck.Get(filterParams => null) */
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
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.Questions_WithImages())
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
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.Questions_MultipleChoice())
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
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.QuestionsInOtherPeopleWuwi())
            },
            
            //Sets
            new BadgeType
            {
                Key = "DonDoItSelf",
                Name = "IchMachsDochNichtSelbst",
                Description = "{badgePoints} Frages�tze mit mehr als 10 Fragen, mit mind. 1 fremden Frage zusammengestellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Sets),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.SetUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp =>
                {
                    var sets = fp.SetsWithAtLeast10Questions();

                    return sets.Count(s => 
                        s.QuestionsInSet.Any(y => 
                            y.Question.Creator.Id != fp.CurrentUser.Id)
                    );
                })
            },

            //Categories
            new BadgeType
            {
                Key = "FamilyFriend",
                Name = "Familienfreund",
                Description = "{badgePoints} Eltern-/Kindkategorien verkn�pft",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(20),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(400)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.CategoryUpdateOrCreate },
                /* AwardCheck = WE DO NOT SAVE WHO CREATED CONNECTIONS */
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
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.MaxAddedQuestionsToCategory())
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
                Description = "{badgePoints} Fragen im eigenen Wunschwissen hinzugef�gt",
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
                Name = "Arch�ologe",
                Description = "{badgePoints} Fragen zum Wuwi hinzugef�gt, die �lter als 2 Jahre sind",
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
                Description = "{badgePoints} fremde Fragen zum Wunschwissen hinzugef�gt, die neuer als 24h sind.",
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
                Name = "DankeF�rDieHilfe",
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
                Description = "{badgePoints} Tage am St�ck gelernt [nicht: Frage beantwortet]",
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